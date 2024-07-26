using UnityEngine;

namespace Utilities
{
    public static class MouseExtension
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public static Vector3 GetMouseInWorldPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            return Camera.main.ScreenPointToRay(mousePos).GetPoint(500f);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}