using Manager;
using Player;
using UnityEngine;

namespace Structures.Storages
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
    public abstract class Storage : MonoBehaviour, IStorage
    {
        public Vector3 Position => transform.position;
        public ResourcesManager.ResourceType GetStorageType { get; protected set; }
        public EFaction Faction { get; private set; }
        private void Awake()
        {
            var navMeshObstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
            navMeshObstacle.carving = true;
            navMeshObstacle.carveOnlyStationary = true;
        }
        
        public void SetFaction(EFaction faction)
        {
            Faction = faction;
        }
    }
}