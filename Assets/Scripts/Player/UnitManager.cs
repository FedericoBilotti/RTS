using System;
using System.Collections.Generic;
using System.Linq;
using Units;
using Units.Formations;
using Units.Resources;
using Units.Villagers;
using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitManager : SingletonAutoGenerated<UnitManager>
    {
        [SerializeField] private RectTransform _selectionBox;
        [SerializeField] private GameObject _selectionBoxMesh;

        private IController _controller;
        private FormationManager _formationManager;
        private UnitManagerVisual _unitManagerVisual;
        private readonly HashSet<Unit> _selectedUnits = new();

        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();

            _unitManagerVisual = new UnitManagerVisual(_selectionBox);
            _controller = new PlayerUnitController(this, _unitManagerVisual, _selectionBoxMesh);

            _formationManager = GetComponent<FormationManager>();
        }

        private void Update() => _controller.ArtificialUpdate();
        private void FixedUpdate() => _controller.ArtificialFixedUpdate();

        public bool IsUnitSelected(Unit unit) => _selectedUnits.Contains(unit);

        public void SetResourceToWorkUnits(Resource resource)
        {
            IEnumerable<Villager> villagers = _selectedUnits.OfType<Villager>(); // Crear listas de cada tipo específico, asi no se filtra cada vez que necesita lista de villagers.
            
            foreach (Villager selectedUnit in villagers)
            {
                selectedUnit.SetResource(resource);
            }
        }

        public void MoveUnitsInFormation(Vector3 desiredPosition)
        {
            List<Vector3> positions = _formationManager.GetActualFormation(desiredPosition, _selectedUnits.ToList());

            int i = 0;
            foreach (Unit selectedUnit in _selectedUnits)
            {
                Vector3 position = positions[i];
                selectedUnit.SetDestination(position);
                i = (i + 1) % positions.Count;
            }
        }

        public void MoveUnits(Vector3 desiredPosition)
        {
            foreach (Unit selectedUnit in _selectedUnits)
            {
                selectedUnit.SetDestination(desiredPosition);
            }
        }

        public void StopCoroutinesInUnits()
        {
            foreach (Unit unit in _selectedUnits)
            {
                unit.StopAllCoroutines();
            }
        }

        public void AddUnit(Unit unit)
        {
            _selectedUnits.Add(unit);
            unit.UnitVisual.SelectUnit();
        }

        public void RemoveUnity(Unit unit)
        {
            if (!_selectedUnits.Contains(unit)) return;

            _selectedUnits.Remove(unit);
            unit.UnitVisual.DeselectUnit();
        }

        public void ClearUnits()
        {
            _selectedUnits.ForEach(unit => unit.UnitVisual.DeselectUnit());
            _selectedUnits.Clear();
        }

        private void OnDrawGizmos()
        {
            _controller?.DrawGizmo();
        }
    }
}