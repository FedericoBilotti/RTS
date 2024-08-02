namespace Units.Villagers.States
{
    public class WorkVillager : BaseStateVillager
    {
        private IWork _work;
        
        public WorkVillager(Villager villager) : base(villager) { }
        
        
    }

    public interface IWork
    {
        
        void PlayAnimation();
    }
}