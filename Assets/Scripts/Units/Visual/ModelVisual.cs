using UnityEngine;

namespace Units.Visual
{
    public class ModelVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _selector;
        private ISelectable _selectable;

        private void Awake() => _selectable = GetComponent<ISelectable>();

        private void OnEnable()
        {
            _selector.SetActive(false);
            
            _selectable.OnSelectUnit += SelectUnit;
            _selectable.OnDeselectUnit += DeselectUnit;
        }
        
        private void OnDisable()
        {
            _selector.SetActive(false);
            
            _selectable.OnSelectUnit -= SelectUnit;
            _selectable.OnDeselectUnit -= DeselectUnit;
        }

        private void SelectUnit() => _selector.SetActive(true);
        private void DeselectUnit() => _selector.SetActive(false);
    }
}