using System.Collections;

namespace Units.Work
{
    public interface IWorkable
    {
        bool IsWorking { get; }
        
        /// <summary>
        /// Animation to play
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        IEnumerator PlayAnimation(Unit unit);
    }
}