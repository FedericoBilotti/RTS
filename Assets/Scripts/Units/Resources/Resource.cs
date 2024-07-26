using System.Collections;
using Units.Resources.ScriptableObjects;
using Units.Work;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Resources
{
    [RequireComponent(typeof(NavMeshObstacle))]
    public abstract class Resource : MonoBehaviour, IResource
    {
        [SerializeField] protected ResourceSO resource;
        protected int actualAmountOfResource;

        private void Awake() => actualAmountOfResource = resource.TotalAmountOfResource;

        public abstract UnitType UnitDesired();
        public abstract IWorkable AssignWork();
        
        /// <summary>
        /// Give the resource to the player in a specific time.
        /// </summary>
        /// <returns>The amount of resource given</returns>
        public abstract IEnumerator ProvideResource();
        
        public int GetResourceAmountToGive() => resource.AmountToGive;
    }
}