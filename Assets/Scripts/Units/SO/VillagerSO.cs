using System;
using Manager;
using UnityEngine;

namespace Units.SO
{
    [CreateAssetMenu(menuName = "Units/Stats/Villager", fileName = "VillagerSO", order = 0)]
    public class VillagerSO : UnitSO
    {
        [SerializeField] private int _stoppingDistanceToWork = 1;
        [SerializeField] private InventoryMax[] _inventoryMaxes;
        
        public int StoppingDistanceToWork => _stoppingDistanceToWork;

        /// <summary>
        /// Check out if the inventory of the specificed resource is full
        /// </summary>
        /// <param name="desiredResourceType"></param>
        /// <param name="actualAmountInInventory"></param>
        /// <returns></returns>
        public bool IsInventoryFull(ResourcesManager.ResourceType desiredResourceType, int actualAmountInInventory)
        {
            float total = 0;
            InventoryMax resourceInventoryType = _inventoryMaxes[0];

            foreach (InventoryMax inventoryMax in _inventoryMaxes)
            {
                if (inventoryMax.ResourceType != desiredResourceType) continue;

                total = actualAmountInInventory;
                resourceInventoryType = inventoryMax;
            }

            return total >= resourceInventoryType.MaxAmount;
        }

        [Serializable]
        public struct InventoryMax
        {
            [SerializeField] private ResourcesManager.ResourceType _resourceType;
            [SerializeField] private int _maxAmount;

            public ResourcesManager.ResourceType ResourceType => _resourceType;
            public int MaxAmount => _maxAmount;
        }
    }
}