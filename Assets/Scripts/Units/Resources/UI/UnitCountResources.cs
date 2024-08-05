using System.Collections.Generic;
using EventSystem.Channel;
using Manager;
using UnityEngine;

namespace Units.Resources.UI
{
    public class UnitCountResources : MonoBehaviour
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

        // This is to add villager int the villager EventListener
        public void ChangeVillagerText(ResourceEvent resourceEvent)
        {
            _resources[resourceEvent.resourceType].SetText(resourceEvent.amount);
        }
    }
}