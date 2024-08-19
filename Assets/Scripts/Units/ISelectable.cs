using System;

namespace Units
{
    public interface ISelectable
    {
        public event Action OnSelectUnit;
        public event Action OnDeselectUnit;

        public void SelectUnit();
        public void DeselectUnit();
    }
}