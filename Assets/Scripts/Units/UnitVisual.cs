using UnityEngine;

namespace Units
{
    public class UnitVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _selector;
        private Unit _unit;

        private void Awake() => _unit = GetComponent<Unit>();

        private void OnEnable()
        {
            _selector.SetActive(false);
            
            _unit.onSelectUnit += SelectUnit;
            _unit.onDeselectUnit += DeselectUnit;
        }
        
        private void OnDisable()
        {
            _selector.SetActive(false);
            
            _unit.onSelectUnit -= SelectUnit;
            _unit.onDeselectUnit -= DeselectUnit;
        }

        private void SelectUnit() => _selector.SetActive(true);
        private void DeselectUnit() => _selector.SetActive(false);
    }
}