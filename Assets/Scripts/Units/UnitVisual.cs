using UnityEngine;

namespace Units
{
    public class UnitVisual
    {
        private readonly Unit _unit;
        private readonly GameObject _selector;

        public UnitVisual(Unit unit, GameObject selector)
        {
            _selector = selector;
            _selector.SetActive(false);
            
            _unit = unit;
            _unit.onSelectUnit += SelectUnit;
            _unit.onDeselectUnit += DeselectUnit;
        }
        
        ~UnitVisual()
        {
            _selector.SetActive(false);
            _unit.onSelectUnit -= SelectUnit;
            _unit.onDeselectUnit -= DeselectUnit;
        }

        private void SelectUnit() => _selector.SetActive(true);
        private void DeselectUnit() => _selector.SetActive(false);
    }
}