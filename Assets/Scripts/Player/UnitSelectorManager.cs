using Manager;
using Units;
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

                _unitManager.AddUnit(unit);
            }
        }

        public void AddSingleUnit(float mouseDownTime, bool isShiftPressed)
        {
            bool unit = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 500f, GameManager.Instance.GetUnitStructureLayer());

            if (!unit)
            {
                if (mouseDownTime + DELAY_TO_SELECT_UNITS < Time.time)
                    return;                                     // This is to prevent the player from deselecting at the moment to select multiples unit without the shift
                if (!isShiftPressed) _unitManager.ClearUnits(); // This is to clear the selected units if the player touches something, like the ground
                return;
            }

            if (!hit.transform.TryGetComponent(out Unit unitComponent)) return;

            if (isShiftPressed)
            {
                if (RemoveUnitSelected(unitComponent)) return;

                _unitManager.AddUnit(unitComponent); // if not selected, add it and return
                return;
            }

            _unitManager.ClearUnits();
            _unitManager.AddUnit(unitComponent);
        }

        public bool RemoveUnitSelected(Unit unitComponent)
        {
            if (!_unitManager.IsUnitSelected(unitComponent)) return false;

            _unitManager.RemoveUnity(unitComponent);
            return true;
        }
    }
}