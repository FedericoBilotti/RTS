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
        private int _actualAmountOfResource;

        private void Awake() => _actualAmountOfResource = resource.TotalAmountOfResource;

        public ResourcesManager.ResourceType GetResourceType() => resource.ResourceType;
        public Unit.UnitType GetUnitDesired() => resource.DesiredUnitType;
        public float GetTimeToGiveResource() => resource.TimeToGiveResource;
        public int GetActualAmount() => _actualAmountOfResource;

        /// <summary>
        /// Give the resource to the player in a specific time.
        /// </summary>
        /// <returns>The amount of resource given</returns>
        public virtual int ProvideResource()
        {
            int amount = resource.AmountToGive;
            _actualAmountOfResource -= amount;

            if (_actualAmountOfResource <= 0)
            {
                gameObject.SetActive(false);
            }

            return amount;
        }
    }
}