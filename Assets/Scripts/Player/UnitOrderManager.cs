using Structures.Storages;
using Units.Resources;
using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitOrderManager
    {
        private readonly UnitManager _unitManager;

        public UnitOrderManager(UnitManager unitManager) => _unitManager = unitManager;

        public void ControlUnits()
        {
            bool hitSomething = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 100f);
            if (!hitSomething) return;

            Vector3 destination = hit.point;

            if (hit.transform.TryGetComponent(out IWork work))
            {
                AssignWorkToVillagers(work);
            }
            else if (hit.transform.TryGetComponent(out IStorage storage))
            {
                if (storage.Faction != _unitManager.Faction) return;
                
                AssignStorageToVillagers(storage);
            }
            else if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                if (storage.Faction == _unitManager.Faction) return;
                
                destination = damageable.Position;
                MoveUnits(destination);
            }
            else
            {
                MoveUnitsInFormation(destination);
                AssignWorkToVillagers(null);
            }
        }

        private void AssignWorkToVillagers(IWork resource) => _unitManager.SetResourceToWorkUnits(resource);
        private void AssignStorageToVillagers(IStorage storage) => _unitManager.SetStorage(storage);
        private void MoveUnits(Vector3 destination) => _unitManager.MoveUnits(destination);
        private void MoveUnitsInFormation(Vector3 destination) => _unitManager.MoveUnitsInFormation(destination);
    }
}