using System.Collections.Generic;
using Player.OrderActions;
using UnityEngine;
using Utilities;

namespace Player
{
    public class UnitOrderManager
    {
        private readonly UnitManager _unitManager;

        private readonly List<IOrderStrategy> _actions;

        public UnitOrderManager(UnitManager unitManager)
        {
            _unitManager = unitManager;

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
                if (action.Execute(_unitManager, hit)) break; // Se va a ejecutar cada acción hasta que se cumpla una.
            }
        }
    }
}