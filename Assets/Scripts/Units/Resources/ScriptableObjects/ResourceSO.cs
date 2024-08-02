using Manager;
using UnityEngine;

namespace Units.Resources.ScriptableObjects
{
    public abstract class ResourceSO : ScriptableObject
    {
        [SerializeField] private int _totalAmountOfResource = 500;
        [SerializeField] private int _amountToGive = 20;
        [SerializeField] private float _timeToGiveResource;
        [SerializeField] private LayerMask _resourceLayerMask;
        [SerializeField] private ResourcesManager.ResourceType _resourceType;

        public int TotalAmountOfResource => _totalAmountOfResource;
        public int AmountToGive => _amountToGive;
        public float TimeToGiveResource => _timeToGiveResource;
        public LayerMask ResourceLayerMask => _resourceLayerMask;
        public ResourcesManager.ResourceType ResourceType => _resourceType;
    }
}