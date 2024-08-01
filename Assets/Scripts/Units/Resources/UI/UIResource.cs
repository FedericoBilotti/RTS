using System;
using System.Collections.Generic;
using EventSystem;
using Manager;
using TMPro;
using UnityEngine;

namespace Units.Resources.UI
{
    public class UIResource : MonoBehaviour
    {
        [SerializeField] private TextResource[] _resourceTexts;

        private readonly Dictionary<ResourcesManager.ResourceType, TextResource> _resources = new();

        private void Awake() => AddTextToDictionary();

        private void AddTextToDictionary()
        {
            foreach (TextResource resourceText in _resourceTexts)
            {
                _resources.Add(resourceText.ResourceType, resourceText);
            }
        }

        public void ChangeResourceText(ResourceEvent resourceEvent)
        {
            _resources[resourceEvent.resourceType].SetText(resourceEvent.amount);
        }

        [Serializable]
        private class TextResource
        {
            [SerializeField] private ResourcesManager.ResourceType _resourceType;
            [SerializeField] private TextMeshProUGUI _resourceText;
            [SerializeField] private string _preFix;

            public ResourcesManager.ResourceType ResourceType => _resourceType;

            public void SetText(int amount)
            {
                _resourceText.text = $"{_preFix} {amount}";
            }
        }
    }
}