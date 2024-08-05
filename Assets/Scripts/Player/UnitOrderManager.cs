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
                AssignWorkToSelectedUnits(work);
            }
            else if (hit.transform.TryGetComponent(out IStorage storage))
            {
                AssignStorage(storage);
            }
            else if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                destination = damageable.Position;
                MoveUnits(destination);
            }
            else
            {
                MoveUnitsInFormation(destination);
                AssignWorkToSelectedUnits(null);
            }
        }

        private void AssignWorkToSelectedUnits(IWork resource) => _unitManager.SetResourceToWorkUnits(resource);
        private void AssignStorage(IStorage storage) => _unitManager.SetStorage(storage);
        private void MoveUnits(Vector3 destination) => _unitManager.MoveUnits(destination);
        private void MoveUnitsInFormation(Vector3 destination) => _unitManager.MoveUnitsInFormation(destination);
    }
}