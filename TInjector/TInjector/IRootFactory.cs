// TInjector: TInjector
// IRootFactory.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-18 11:32 AM

using TInjector.Registration;

namespace TInjector
{
    /// <summary>
    ///     Functionality required for all root factory objects.
    ///     Note: Implementers are not required to be thread-safe.
    /// </summary>
    public interface IRootFactory
    {
        /// <summary>
        ///     Save modules to generate registrations when the factory generates a root.
        /// </summary>
        /// <param name="modules"></param>
        void Register(params IRegistrationModule[] modules);

        /// <summary>
        ///     Get a root from this factory.
        /// </summary>
        /// <returns></returns>
        IRoot GetRoot();
    }
}