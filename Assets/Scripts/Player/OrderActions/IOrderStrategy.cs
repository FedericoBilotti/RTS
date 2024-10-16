using UnityEngine;

namespace Player.OrderActions
{
    public interface IOrderStrategy
    {
        bool Execute(PlayerManager playerManager, RaycastHit hit);
    }
}