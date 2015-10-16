using TInjector.Initialization;

namespace TInjector
{
    /// <summary>
    /// Functionality required for all root factory objects.
    /// </summary>
    public interface IRootFactory
    {
        /// <summary>
        /// Save modules to generate registrations when the factory generates a root.
        /// </summary>
        /// <param name="modules"></param>
        void Register(params IRegistrationModule[] modules);

        /// <summary>
        /// Get a root from this factory.
        /// </summary>
        /// <returns></returns>
        IRoot GetRoot();
    }
}
