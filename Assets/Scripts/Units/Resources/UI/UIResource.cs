using System.Collections.Generic;
using EventSystem.Channel;
using Manager;
using UnityEngine;

namespace Units.Resources.UI
{
    public class UIResource : MonoBehaviour
    {
        [SerializeField] private TextResource[] _resourceTexts;

        private readonly Dictionary<ResourcesManager.ResourceType, TextResource> _resources = new();
        
        private void OnValidate()
        {
            foreach (TextResource resourceText in _resourceTexts)
            {
                resourceText.name = resourceText.ResourceType.ToString();
            }
        }

        private void Awake() => AddTextToDictionary();

        private void AddTextToDictionary()
        {
            foreach (TextResource resourceText in _resourceTexts)
            {
                _resources.Add(resourceText.ResourceType, resourceText);
            }
        }

        // This is to add in the Resource EventListener. It´s called in the inspector.
        public void ChangeResourceText(ResourceEvent resourceEvent)
        {
            _resources[resourceEvent.resourceType].SetText(resourceEvent.amount);
        }
    }
}