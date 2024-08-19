using System;
using Manager;
using Player;
using UnityEngine;

namespace Units.Structures.Storages
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
    public abstract class Storage : MonoBehaviour, IStorage, ISelectable
    {
        [SerializeField] private EFaction _faction;

        public Vector3 Position => transform.position;
        public ResourcesManager.ResourceType StorageType { get; protected set; }
        
        public event Action OnSelectUnit = delegate { };
        public event Action OnDeselectUnit = delegate { };

        private void Awake()
        {
            var navMeshObstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
            navMeshObstacle.carving = true;
            navMeshObstacle.carveOnlyStationary = true;
        }
        
        public void SetFaction(EFaction faction) => _faction = faction;
        public EFaction GetFaction() => _faction;
        
        public void SelectUnit() => OnSelectUnit.Invoke();
        public void DeselectUnit() => OnDeselectUnit.Invoke();
    }
}