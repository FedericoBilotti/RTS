using UnityEngine;

namespace Units.Villagers.States
{
    public class Idle : BaseState
    {
        private readonly Villager _villager;

        public Idle(Villager villager)
        {
            _villager = villager;
        }
        
        public override void OnEnter()
        {
            _villager.SetName("Idle");
        }
        // Play Idle Animation
    }
}