using Units.Work;

namespace Units.Resources
{
    public interface IResource
    {
        /// <summary>
        /// Assign the type of work to be done, for the specific resource.
        /// </summary>
        /// <returns>Return the work</returns>
        IWorkable AssignWork();

        /// <summary>
        /// Unit Desired to do the work
        /// </summary>
        /// <returns></returns>
        UnitType UnitDesired();
    }
}