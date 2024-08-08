using Manager;
using Structures.Storages;
using Units;
using Units.Villagers;
using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitSelectorManager
    {
        private readonly UnitVisualManager _unitVisualManager;
        private readonly UnitManager _unitManager;

        private const float DELAY_TO_SELECT_UNITS = .1f;

        public UnitSelectorManager(UnitManager unitManager)
        {
            _unitManager = unitManager;
        }

        public void GetUnitInArea(Vector3 center, Vector3 size, bool isShiftPressed)
        {
            size.Set(Mathf.Abs(size.x / 2), 1, Mathf.Abs(size.z / 2));
            Collider[] collisions = Physics.OverlapBox(center, size, Quaternion.identity, GameManager.Instance.GetUnitLayer());

            if (!isShiftPressed)
            {
                _unitManager.ClearUnits();
            }

            foreach (Collider hit in collisions)
            {
                if (!hit.transform.TryGetComponent(out Unit unit)) continue;
                if (unit.GetFaction() != _unitManager.Faction) continue;

                if (unit is Villager villager)
                {
                    _unitManager.AddSelectedVillager(villager);
                }

                _unitManager.AddUnit(unit);
            }
        }

        public void AddSingleUnit(float mouseDownTime, bool isShiftPressed)
        {
            bool unitOrStructure = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 500f, GameManager.Instance.GetUnitAndStructureLayer());

            if (!unitOrStructure)
            {
                if (mouseDownTime + DELAY_TO_SELECT_UNITS < Time.time)
                    return;                                     // This is to prevent the player from deselecting at the moment to select multiples unit without the shift
                if (!isShiftPressed) _unitManager.ClearUnits(); // This is to clear the selected units if the player touches something, like the ground
                return;
            }

            if (!hit.transform.TryGetComponent(out Unit unitComponent)) return;

            // Add more than one unit
            if (isShiftPressed)
            {
                if (RemoveUnitSelected(unitComponent)) return;

                AddUnit(unitComponent);
                return;
            }

            // Add only one unit
            _unitManager.ClearUnits();
            AddUnit(unitComponent);
        }

        private void AddUnit(Unit unitComponent)
        {
            if (unitComponent is Villager villager) _unitManager.AddSelectedVillager(villager);

            _unitManager.AddUnit(unitComponent);
        }

        private bool RemoveUnitSelected(Unit unitComponent)
        {
            if (!_unitManager.IsUnitSelected(unitComponent)) return false;

            if (unitComponent is Villager villager) _unitManager.RemoveSelectedVillager(villager);

            _unitManager.RemoveUnity(unitComponent);
            return true;
        }
    }
}