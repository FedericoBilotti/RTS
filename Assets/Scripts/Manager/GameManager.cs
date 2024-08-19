using System.Collections.Generic;
using System.Linq;
using Units;
using Units.Structures;
using Units.Structures.Storages;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        private readonly List<Center> _centers = new();
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
        /// <param name="unit"></param>
        /// <returns></returns>
        public Center NearCenter(Unit unit)
        {
            // Tmb compararia los q son de mi facción con un .Where
            return _centers.OrderBy(center => (center.transform.position - unit.transform.position).sqrMagnitude).FirstOrDefault();
        }

        /// <summary>
        /// Returns the nearest storage of the desired type from the unit if it exists, otherwise returns null.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="desiredStorage"></param>
        /// <returns></returns>
        public IStorage NearStorage(Unit unit, ResourcesManager.ResourceType desiredStorage)
        {
            // Tmb compararia los q son de mi facción
            return _storages.Where(x => x.StorageType == desiredStorage).OrderBy(x => Vector3.Distance(unit.transform.position, x.Position)).FirstOrDefault();
        }
    }
}