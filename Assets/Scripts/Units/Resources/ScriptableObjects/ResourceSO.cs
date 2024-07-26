using UnityEngine;

namespace Units.Resources.ScriptableObjects
{
    public abstract class ResourceSO : ScriptableObject
    {
        [SerializeField] private int _totalAmountOfResource = 500;
        [SerializeField] private int _amountToGive = 20;
        [SerializeField] private float _timeToGiveResource;
        [SerializeField] private ResourceType _resourceType;

        public int TotalAmountOfResource => _totalAmountOfResource;
        public int AmountToGive => _amountToGive;
        public float TimeToGiveResource => _timeToGiveResource;
        public ResourceType ResourceType => _resourceType;
    }

    public enum ResourceType
    {
        Wood,
        Meal,
        Gold,
    }
}