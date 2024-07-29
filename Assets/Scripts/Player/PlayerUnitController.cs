using Manager;
using Units;
using Units.Resources;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerUnitController : IController
    {
        private readonly UnitManager _unitManager;
        private readonly UnitManagerVisual _unitManagerVisual;
        private readonly GameObject _selectionArea;
        private readonly Camera _camera;

        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _firstPosition = Vector3.zero;
        private Vector3 _finalPosition = Vector3.zero;

        private float _mouseDownTime;
        private const float DELAY_TO_SELECT_UNITS = .1f;

        public PlayerUnitController(UnitManager unitManager, UnitManagerVisual unitManagerVisual, GameObject selectionArea)
        {
            _unitManager = unitManager;
            _unitManagerVisual = unitManagerVisual;
            _selectionArea = selectionArea;
            _camera = Camera.main;
        }

        public void ArtificialUpdate()
        {
            if (RightMouseButtonDown())
            {
                ControlUnits();
            }

            if (LeftMouseButtonDown())
            {
                _unitManagerVisual.SetActiveBox(true);
                _unitManagerVisual.SetSizeBox(Vector2.zero);
                _startPosition = Input.mousePosition;
                _firstPosition = MouseExtension.GetMouseInWorldPosition();

                _mouseDownTime = Time.time;
            }
            else if (LeftMouseButton() && _mouseDownTime + DELAY_TO_SELECT_UNITS < Time.time)
            {
                //_unitManagerVisual.ResizeSelectionBox(_startPosition);
                
                _finalPosition = MouseExtension.GetMouseInWorldPosition();
                Vector3 dragCenter = (_firstPosition + _finalPosition) / 2;
                Vector3 dragSize = _finalPosition - _firstPosition;
                dragSize.y = 1;
                
                ScaleBox(dragCenter, dragSize);
                GetUnitInBox(dragCenter, dragSize);
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

            _unitManager.StopCoroutinesInUnits();
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
            else
            {
                MoveUnitsInFormation(destination);
            }
        }

        private void AssignWorkToSelectedUnits(Resource resource) => _unitManager.SetResourceToWorkUnits(resource, resource.GetUnitDesired());
        private void MoveUnitsInFormation(Vector3 destination) => _unitManager.MoveUnitsInFormation(destination);
        private void MoveUnits(Vector3 destination) => _unitManager.MoveUnits(destination);

        private void ScaleBox(Vector3 center, Vector3 size)
        {
            _selectionArea.transform.position = center;
            _selectionArea.transform.localScale = size + Vector3.up;
        }

        private void GetUnitInBox(Vector3 center, Vector3 size)
        {
            RaycastHit[] collisions = Physics.BoxCastAll(center, size, Vector3.one, Quaternion.identity, 0, GameManager.Instance.GetUnitLayer());
            
            _unitManager.ClearUnits();
            
            foreach (RaycastHit hit in collisions)
            {
                if (!hit.transform.TryGetComponent(out Unit unit)) continue;
                    
                _unitManager.AddUnit(unit);
            }
        }

        private void GetUnitInSelectionBox()
        {
            Bounds bounds = _unitManagerVisual.CreateBounds();

            for (int i = 0; i < _unitManager.GetTotalUnits().Count; ++i)
            {
                // Transform the unit position to screen space to use it with the bounds
                // This going to generate troubles when having a lot of units
                if (IsUnitInSelectionBox(_camera.WorldToScreenPoint(_unitManager.GetTotalUnits()[i].transform.position), bounds))
                {
                    _unitManager.AddUnit(_unitManager.GetTotalUnits()[i]);
                }
                else if (!LeftShiftButton())
                {
                    _unitManager.RemoveUnity(_unitManager.GetTotalUnits()[i]);
                }
            }
        }

        private bool IsUnitInSelectionBox(Vector3 unitPos, Bounds bounds)
        {
            return unitPos.x > bounds.min.x && unitPos.x < bounds.max.x && unitPos.y > bounds.min.y && unitPos.y < bounds.max.y;
        }

        private void AddSingleUnit(float mouseDownTime)
        {
            bool unit = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 500f, GameManager.Instance.GetUnitStructureLayer());

            if (!unit)
            {
                if (mouseDownTime + DELAY_TO_SELECT_UNITS > Time.time && !LeftShiftButton()) _unitManager.ClearUnits();
                return;
            }

            if (!hit.transform.TryGetComponent(out Unit unitComponent)) return;

            if (LeftShiftButton())
            {
                // If is selected, remove it
                if (_unitManager.IsUnitSelected(unitComponent))
                {
                    _unitManager.RemoveUnity(unitComponent);
                    return;
                }

                // if not selected, add it
                _unitManager.AddUnit(unitComponent);
                return;
            }

            _unitManager.ClearUnits();
            _unitManager.AddUnit(unitComponent);
        }

        private bool RightMouseButtonDown() => Input.GetMouseButtonDown(1);
        private bool LeftMouseButtonDown() => Input.GetMouseButtonDown(0);
        private bool LeftMouseButton() => Input.GetMouseButton(0);
        private bool LeftMouseButtonUp() => Input.GetMouseButtonUp(0);
        private bool LeftShiftButton() => Input.GetKey(KeyCode.LeftShift);

        public void DrawGizmo()
        {
            Vector3 center = (_firstPosition + _finalPosition) / 2;
            Vector3 size = _finalPosition - _firstPosition;
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