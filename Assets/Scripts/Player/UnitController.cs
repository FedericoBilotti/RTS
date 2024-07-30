using Manager;
using Structures;
using Units;
using Units.Resources;
using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitController : IController
    {
        private readonly UnitManager _unitManager;
        private readonly UnitManagerVisual _unitManagerVisual;

        private Vector3 _firstPosition = Vector3.zero;
        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _endPosition = Vector3.zero;

        private float _mouseDownTime;
        private const float DELAY_TO_SELECT_UNITS = .1f;

        public UnitController(UnitManager unitManager, UnitManagerVisual unitManagerVisual)
        {
            _unitManager = unitManager;
            _unitManagerVisual = unitManagerVisual;
            _unitManagerVisual.SetActiveBox(false);
        }

        public void ArtificialUpdate()
        {
            if (RightMouseButtonDown())
            {
                ControlUnits();
            }

            if (LeftMouseButtonDown())
            {
                _firstPosition = Input.mousePosition;
                _startPosition = MouseExtension.GetMouseInWorldPosition();
                _endPosition = _startPosition;

                _mouseDownTime = Time.time;
            }
            else if (LeftMouseButton())
            {
                _endPosition = MouseExtension.GetMouseInWorldPosition();

                if (Vector3.Distance(_startPosition, _endPosition) < 1) return;

                //_selectionArea.SetActive(true);
                Vector3 dragCenter = (_startPosition + _endPosition) / 2;
                Vector3 dragSize = _endPosition - _startPosition;
                dragSize.y = 1;

                //ScaleBox(dragCenter, dragSize);
                _unitManagerVisual.SetActiveBox(true);
                _unitManagerVisual.ResizeSelectionBox(_firstPosition);
                GetUnitInArea(dragCenter, dragSize);
            }
            else if (LeftMouseButtonUp())
            {
                _unitManagerVisual.SetActiveBox(false);
                AddSingleUnit(_mouseDownTime);
                _mouseDownTime = 0;
            }
        }

        public void ArtificialFixedUpdate() { }

        private void ControlUnits()
        {
            bool hitSomething = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 500f);
            if (!hitSomething) return;

            Vector3 destination = hit.point;

            if (hit.transform.TryGetComponent(out Resource resource))
            {
                AssignWorkToSelectedUnits(resource);
            }
            else if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                destination = damageable.Position;
                MoveUnits(destination);
            }
            else if (hit.transform.TryGetComponent(out Center center))
            {
                MoveToCenter(center);
            }
            else
            {
                AssignWorkToSelectedUnits(null);
                MoveUnitsInFormation(destination);
            }
        }

        private void MoveToCenter(Center center) => _unitManager.MoveToCenter(center);
        private void AssignWorkToSelectedUnits(Resource resource) => _unitManager.SetResourceToWorkUnits(resource);
        private void MoveUnitsInFormation(Vector3 destination) => _unitManager.MoveUnitsInFormation(destination);
        private void MoveUnits(Vector3 destination) => _unitManager.MoveUnits(destination);

        private void GetUnitInArea(Vector3 center, Vector3 size)
        {
            size.Set(Mathf.Abs(size.x / 2), 1, Mathf.Abs(size.z / 2));
            Collider[] collisions = Physics.OverlapBox(center, size, Quaternion.identity, GameManager.Instance.GetUnitLayer());

            if (!LeftShiftButton())
            {
                _unitManager.ClearUnits();
            }

            foreach (Collider hit in collisions)
            {
                if (!hit.transform.TryGetComponent(out Unit unit)) continue;

                _unitManager.AddUnit(unit);
            }
        }

        private void AddSingleUnit(float mouseDownTime)
        {
            bool unit = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 500f, GameManager.Instance.GetUnitStructureLayer());

            if (!unit)
            {
                if (mouseDownTime + DELAY_TO_SELECT_UNITS < Time.time) return; // This is to prevent the player from deselecting at the moment to select multiples unit without the shift
                if (!LeftShiftButton()) _unitManager.ClearUnits(); // This is to clear the selected units if the player touches something, like the ground
                return;
            }

            if (!hit.transform.TryGetComponent(out Unit unitComponent)) return;

            if (LeftShiftButton())
            {
                if (RemoveUnitSelected(unitComponent)) return;

                _unitManager.AddUnit(unitComponent); // if not selected, add it and return
                return;
            }

            _unitManager.ClearUnits();
            _unitManager.AddUnit(unitComponent);
        }

        private bool RemoveUnitSelected(Unit unitComponent)
        {
            if (!_unitManager.IsUnitSelected(unitComponent)) return false;
            
            _unitManager.RemoveUnity(unitComponent);
            return true;
        }

        private bool RightMouseButtonDown() => Input.GetMouseButtonDown(1);
        private bool LeftMouseButtonDown() => Input.GetMouseButtonDown(0);
        private bool LeftMouseButton() => Input.GetMouseButton(0);
        private bool LeftMouseButtonUp() => Input.GetMouseButtonUp(0);
        private bool LeftShiftButton() => Input.GetKey(KeyCode.LeftShift);

        public void DrawGizmo()
        {
            Vector3 center = (_startPosition + _endPosition) / 2;
            Vector3 size = _endPosition - _startPosition;
            size.y = 1;
            Gizmos.DrawWireCube(center, size);
        }
    }

    public interface IController
    {
        void ArtificialUpdate();
        void ArtificialFixedUpdate();

        void DrawGizmo();
    }
}