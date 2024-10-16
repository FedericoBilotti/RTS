using System.Collections.Generic;
using Player.OrderActions;
using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitOrderManager
    {
        private readonly PlayerManager playerManager;

        private readonly List<IOrderStrategy> _actions;

        public UnitOrderManager(PlayerManager playerManager)
        {
            this.playerManager = playerManager;

            _actions = new List<IOrderStrategy>
            {
                new WorkAction(),
                new AttackAction(), 
                new StorageAction(), 
                new MoveUnitsInFormation() // Siempre tiene que ser la última
            };
        }

        public void ControlUnits()
        {
            bool hitSomething = Physics.Raycast(MouseExtension.GetMouseRay(), out RaycastHit hit, 100f);
            if (!hitSomething) return;

            foreach (IOrderStrategy action in _actions)
            {
                if (action.Execute(playerManager, hit)) break; // Se va a ejecutar cada acción hasta que se cumpla una.
            }
        }
    }
}