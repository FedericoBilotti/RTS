using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitVisualManager : Singleton<UnitVisualManager>
    {
        [SerializeField] private RectTransform _selectionBox;

        public void ResizeSelectionBox(Vector2 startPosition)
        {
            float width = Input.mousePosition.x - startPosition.x;
             float height = Input.mousePosition.y - startPosition.y;
         
            _selectionBox.anchoredPosition = startPosition + new Vector2(width * 0.5f, height * 0.5f); // Put the pivot in the actual mouse position
            _selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));                // Scale the selection box
        }
        
        public void SetActiveBox(bool param) => _selectionBox.gameObject.SetActive(param);
    }
}