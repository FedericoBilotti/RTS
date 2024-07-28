namespace Units.Villagers.States
{
    public class Moving : BaseState
    {
        private readonly Villager _villager;

        public Moving(Villager villager) => _villager = villager;

        public override void OnEnter()
        {
            _villager.SetResource(null);   
        }
    }
}