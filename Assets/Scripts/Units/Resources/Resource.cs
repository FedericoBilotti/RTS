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

        public ResourcesManager.ResourceType GetResourceType() => resource.ResourceType;
        public UnitType GetUnitDesired() => resource.DesiredUnitType;
        public float GetTimeToGiveResource() => resource.TimeToGiveResource;
        public int GetActualAmount() => actualAmountOfResource;
        
        /// <summary>
        /// Give the resource to the player in a specific time.
        /// </summary>
        /// <returns>The amount of resource given</returns>
        public abstract int ProvideResource();
    }
}