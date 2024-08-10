using Units.Resources.ScriptableObjects;
using UnityEngine;

namespace Units.Resources
{
    public abstract class Resource : MonoBehaviour, IWork
    {
        [SerializeField] private ResourceSO _resourceSO;
        private int _actualAmountOfResource;

        private void Awake() => _actualAmountOfResource = _resourceSO.TotalAmountOfResource;

        /// <summary>
        /// Give the resource to the player in a specific time.
        /// </summary>
        /// <returns>The amount of resource given</returns>
        public virtual int ProvideResource()
        {
            int amount = _resourceSO.AmountToGive;
            _actualAmountOfResource -= amount;

            if (_actualAmountOfResource <= 0)
            {
                gameObject.SetActive(false);
            }

            return amount;
        }

        public ResourceSO GetResourceSO() => _resourceSO;

        public Vector3 Position => transform.position;
        public bool HasResources() => _actualAmountOfResource > 0;
        
        public abstract void PlayAnimation(Unit unit);
    }
}