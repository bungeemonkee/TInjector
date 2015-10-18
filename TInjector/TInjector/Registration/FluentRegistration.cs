// TInjector: TInjector
// FluentRegistration.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-17 8:28 PM

using TInjector.Scope;

namespace TInjector.Registration
{
    public class FluentRegistration<TImplementer> : UnlockedRegistration
    {
        public FluentRegistration() : base(typeof (TImplementer))
        {
        }

        /// <summary>
        ///     Registers an implementer type against the given service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service as which the implementer will be registered.</typeparam>
        /// <returns>This registration object.</returns>
        public FluentRegistration<TImplementer> AsService<TService>()
        {
            // add the service
            AddService(typeof (TService));

            // return this same object
            return this;
        }

        /// <summary>
        ///     Registers an implementation type against all interfaces it implements.
        ///     Note: Some system interfaces are excluded from registration. See the documentation for
        ///     TInjector.Initialization.UnlockedRegistration.AddAllServices.
        /// </summary>
        /// <returns>This registration object.</returns>
        public FluentRegistration<TImplementer> AsAllServices()
        {
            // register all the services
            AddAllServices();

            // return this same object
            return this;
        }

        /// <summary>
        ///     Sets the scope type for this registration.
        /// </summary>
        /// <returns>This registration object.</returns>
        public FluentRegistration<TImplementer> InScope(ScopeType scope)
        {
            // set the scope
            Scope = scope;

            // return this same object
            return this;
        }
    }
}