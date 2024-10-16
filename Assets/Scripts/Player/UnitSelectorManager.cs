using Manager;
using Units;
using Units.Structures;
using Units.Villagers;
using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitSelectorManager
    {
        private readonly UnitVisualManager _unitVisualManager;
        private readonly PlayerManager playerManager;

        private const float DELAY_TO_SELECT_UNITS = .1f;

        public UnitSelectorManager(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public void GetUnitInArea(Vector3 center, Vector3 size, bool isShiftPressed)
        {
            size.Set(Mathf.Abs(size.x / 2), 1, Mathf.Abs(size.z / 2));
            Collider[] collisions = Physics.OverlapBox(center, size, Quaternion.identity, GameManager.Instance.GetUnitLayer());

            if (!isShiftPressed)
            {
                playerManager.ClearUnits();
            }

            foreach (Collider hit in collisions)
            {
                if (!hit.transform.TryGetComponent(out Unit unit)) continue;
                if (unit.GetFaction() != playerManager.Faction) continue;

                playerManager.AddUnit(unit);
            }
        }

        public void AddUnitOrStructure(float mouseDownTime, bool isShiftPressed)
        {
            bool unitOrStructure = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 500f, GameManager.Instance.GetUnitAndStructureLayer());

            if (!unitOrStructure)
            {
                ClearUnitsAndStructureLists(mouseDownTime, isShiftPressed);
                return;
            }

            if (hit.transform.TryGetComponent(out Unit unitComponent))
            {
                HandleUnits(isShiftPressed, unitComponent);
            }
            else if (!hit.transform.TryGetComponent(out Structure structure))
            {
                HandleStructure(isShiftPressed, structure);
            }
        }

        private void ClearUnitsAndStructureLists(float mouseDownTime, bool isShiftPressed)
        {
            // This is to prevent the player from deselecting at the moment to select multiples unit without the shift
            if (mouseDownTime + DELAY_TO_SELECT_UNITS < Time.time) return;
            if (isShiftPressed) return;

            playerManager.ClearUnits();      // This is to clear the selected units if the player touches something, like the ground
            playerManager.ClearStructures(); // This is to clear the selected structures if the player touches something, like the ground
        }

        private void HandleUnits(bool isShiftPressed, Unit unitComponent)
        {
            // Add more than one unit
            if (isShiftPressed)
            {
                if (RemoveUnitSelected(unitComponent)) return;

                AddUnit(unitComponent);
                return;
            }

            // Add only one unit
            playerManager.ClearUnits();
            AddUnit(unitComponent);
        }

        private void HandleStructure(bool isShiftPressed, Structure structure)
        {
            if (!isShiftPressed) playerManager.ClearStructures();
            if (playerManager.IsStructureSelected(structure)) return;

            playerManager.AddStructure(structure);
            Debug.Log($"¿Esta estructura esta seleccionada? {playerManager.IsStructureSelected(structure)}");
        }

        private void AddUnit(Unit unitComponent)
        {
            if (unitComponent is Villager villager) playerManager.AddSelectedVillager(villager);

            playerManager.AddUnit(unitComponent);
        }

        private bool RemoveUnitSelected(Unit unitComponent)
        {
            if (!playerManager.IsUnitSelected(unitComponent)) return false;

            if (unitComponent is Villager villager) playerManager.RemoveSelectedVillager(villager);

            playerManager.RemoveUnity(unitComponent);
            return true;
        }
    }
}