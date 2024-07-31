using StateMachine;
using Units.Villagers;

namespace Units
{
    public class BaseStateVillager : IState
    {
        protected readonly Villager villager;

        protected BaseStateVillager(Villager villager) => this.villager = villager;

        public virtual void OnEnter() { }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnExit() { }
    }
}