using UnityEngine;
using Utilities;

namespace Units.Villagers.States
{
    public class Chop : BaseState
    {
        private readonly CountdownTimer _timer = new(2f);

        public Chop(Villager villager)
        {
            int amount = villager.GetResource().GetResourceAmountToGive();
            
            _timer.onTimerStop += () => villager.SetAmountResource(amount);
            _timer.onTimerStop += () => _timer.Start();
        }

        // Play chop animation
        public override void OnUpdate()
        {
            _timer.Tick(Time.deltaTime);
        }
    }
}