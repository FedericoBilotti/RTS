using UnityEngine;

namespace Utilities
{
    public static class MouseExtension
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public static Vector3 GetMouseInWorldPosition()
        {
            Vector3 position = Vector3.zero;

            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100) ? info.point : position;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}