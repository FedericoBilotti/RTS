using System;
using Manager;
using TMPro;
using UnityEngine;

namespace Units.Resources.UI
{
    [Serializable]
    public class TextResource
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