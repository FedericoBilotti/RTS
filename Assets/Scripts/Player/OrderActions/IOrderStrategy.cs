using UnityEngine;

namespace Player.OrderActions
{
    public interface IOrderStrategy
    {
        bool Execute(UnitManager unitManager, RaycastHit hit);
    }
}