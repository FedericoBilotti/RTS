using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitController : IController
    {
        private readonly UnitOrderManager _unitOrderManager;
        private readonly UnitVisualManager _unitVisualManager;
        private readonly UnitSelectorManager _unitSelectorManager;

        private Vector3 _firstPosition = Vector3.zero;
        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _endPosition = Vector3.zero;

        private float _mouseDownTime;

        public UnitController(UnitVisualManager unitVisualManager, UnitSelectorManager unitSelectorManager, UnitOrderManager unitOrderManager)
        {
            _unitVisualManager = unitVisualManager;
            _unitSelectorManager = unitSelectorManager;
            _unitOrderManager = unitOrderManager;
            _unitVisualManager.SetActiveBox(false);
        }

        public void ArtificialUpdate()
        {
            if (RightMouseButtonDown())
            {
                _unitOrderManager.ControlUnits();
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

                Vector3 dragCenter = (_startPosition + _endPosition) / 2;
                Vector3 dragSize = _endPosition - _startPosition;
                dragSize.y = 1;

                //ScaleBox(dragCenter, dragSize);
                _unitVisualManager.SetActiveBox(true);
                _unitVisualManager.ResizeSelectionBox(_firstPosition);
                _unitSelectorManager.GetUnitInArea(dragCenter, dragSize, LeftShiftButton());
            }
            else if (LeftMouseButtonUp())
            {
                _unitVisualManager.SetActiveBox(false);
                _unitSelectorManager.AddSingleUnit(_mouseDownTime, LeftShiftButton());
                _mouseDownTime = 0;
            }
        }

        public void ArtificialFixedUpdate() { }

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