using Manager;
using Player;
using UnityEngine;

namespace Structures.Storages
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
    public abstract class Storage : MonoBehaviour, IStorage
    {
        [SerializeField] private EFaction _faction;
        
        public EFaction Faction { get; private set; }
        public Vector3 Position => transform.position;
        public ResourcesManager.ResourceType StorageType { get; protected set; }

        private void Awake()
        {
            var navMeshObstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
            navMeshObstacle.carving = true;
            navMeshObstacle.carveOnlyStationary = true;

            Faction = _faction;
        }
        
        public void SetFaction(EFaction faction)
        {
            Faction = faction;
        }
    }
}