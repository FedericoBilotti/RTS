using Manager;
using UnityEngine;

namespace EventSystem
{
    [CreateAssetMenu(menuName = "EventSystem/ResourceChannel", fileName = "ResourceChannel")]
    public class ResourceChannel : EventChannel<ResourceEvent> { }

    public readonly struct ResourceEvent
    {
        public readonly int amount;
        public readonly ResourcesManager.ResourceType resourceType;

        public ResourceEvent(int amount, ResourcesManager.ResourceType resourceType)
        {
            this.amount = amount;
            this.resourceType = resourceType;
        }
    }
}