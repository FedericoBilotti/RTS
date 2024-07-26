using System.Collections;

namespace Units.Work
{
    public interface IWorkable
    {
        bool IsWorking { get; }
        IEnumerator Working(Unit unit);
    }
}