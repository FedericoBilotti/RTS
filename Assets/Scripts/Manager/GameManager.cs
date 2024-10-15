using System.Collections.Generic;
using System.Linq;
using Player;
using Units;
using Units.Structures;
using Units.Structures.Storages;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        private readonly List<Center> _centers = new(); // Podria hacerse una optimización creando dos listas para cada facción y cuando se añaden chequear que facción es.
        private readonly List<IStorage> _storages = new();

        [SerializeField] private LayerMask _unitLayer;
        [SerializeField] private LayerMask _unitStructureLayer;
        [SerializeField] private LayerMask _resourceLayer;
        [SerializeField] private LayerMask _treesLayer;
        [SerializeField] private LayerMask _groundLayer;

        public LayerMask GetUnitLayer() => _unitLayer;
        public LayerMask GetUnitAndStructureLayer() => _unitStructureLayer;
        public LayerMask GetResourceLayer() => _resourceLayer;
        public LayerMask GetTreesLayer() => _treesLayer;
        public LayerMask GetGroundLayer() => _groundLayer;

        public void AddCenter(Center center) => _centers.Add(center);
        public void RemoveCenter(Center center) => _centers.Remove(center);

        public void AddStorage(IStorage storage) => _storages.Add(storage);
        public void RemoveStorage(IStorage storage) => _storages.Remove(storage);

        /// <summary>
        /// Returns the nearest center from the unit if it exists.
        /// </summary>
        /// <param name="unit">Unit for which the nearest center is searched.</param>
        /// <param name="faction">Faction of the unit for which the nearest center is searched.</param>
        /// <returns>Returns the nearest cente.</returns>
        public Center NearCenter(Unit unit, EFaction faction)
        {
            Center center = _centers
                    .Where(x => x.GetFaction() == faction)
                    .OrderBy(center => (center.transform.position - unit.transform.position).sqrMagnitude).FirstOrDefault();
            
            Debug.Log($"¿Center encontrado? { center }");

            return center;
        }

        /// <summary>
        /// Returns the nearest storage of the desired type (centers too) from the unit if it exists.
        /// </summary>
        /// <param name="unit">Unit for which the nearest center is searched.</param>
        /// <param name="faction">Faction of the unit for which the nearest center is searched.</param>
        /// <param name="desiredStorage">Resource type that is searched.</param>
        /// <returns>Returns the nearest storage to the unit.</returns>
        public IStorage NearStorage(Unit unit, EFaction faction, ResourcesManager.ResourceType desiredStorage)
        {
            IStorage storage = _storages
                    .Where(x => x.GetFaction() == faction && x.StorageType == desiredStorage || x.StorageType == ResourcesManager.ResourceType.All)
                    .OrderBy(x => Vector3.Distance(unit.transform.position, x.Position))
                    .FirstOrDefault();

            return storage;
        }
    }
}