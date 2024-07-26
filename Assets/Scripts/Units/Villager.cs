using Units.Work;

namespace Units
{
    public class Villager : Unit
    {
        public override void DoWork(IWorkable work)
        {
            StartCoroutine(work.PlayAnimation(this));
        }
    }
}