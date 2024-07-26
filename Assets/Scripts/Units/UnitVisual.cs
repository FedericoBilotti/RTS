using UnityEngine;

namespace Units
{
    public class UnitVisual
    {
        private readonly GameObject _selector;

        public UnitVisual(GameObject selector)
        {
            _selector = selector;
            _selector.SetActive(false);
        }

        public void SelectUnit() => _selector.SetActive(true);
        public void DeselectUnit() => _selector.SetActive(false);
    }
}