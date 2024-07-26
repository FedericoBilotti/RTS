using System.Collections;
using Manager;
using Units.Resources.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Resources
{
    [RequireComponent(typeof(NavMeshObstacle))]
    public abstract class Resource : MonoBehaviour
    {
        [SerializeField] protected ResourceSO resource;
        protected int actualAmountOfResource;

        private void Awake() => actualAmountOfResource = resource.TotalAmountOfResource;

        public ResourcesManager.ResourceType ResourceType() => resource.ResourceType;
        public UnitType UnitDesired() => resource.UnitType; 
        public int GetResourceAmountToGive() => resource.AmountToGive;
        public int GetActualAmount() => actualAmountOfResource;
        
        /// <summary>
        /// Give the resource to the player in a specific time.
        /// </summary>
        /// <returns>The amount of resource given</returns>
        public abstract IEnumerator ProvideResource();
    }
}