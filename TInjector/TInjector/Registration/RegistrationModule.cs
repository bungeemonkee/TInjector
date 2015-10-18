// TInjector: TInjector
// RegistrationModule.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-18 11:32 AM

using System.Collections.Generic;
using System.Linq;

namespace TInjector.Registration
{
    public abstract class RegistrationModule : IRegistrationModule
    {
        private IList<IRegistration> _registrations;

        public IEnumerable<IRegistration> GenerateRegistrations()
        {
            // since we're using this to maintain state we can only generate one set of registrations at once
            lock (this)
            {
                try
                {
                    // create a new list of registrations
                    _registrations = new List<IRegistration>();

                    // build all the registrations
                    BuildRegistrations();

                    // return the list of registrations
                    return _registrations;
                }
                finally
                {
                    // clean up the registrations
                    _registrations = null;
                }
            }
        }

        protected abstract void BuildRegistrations();

        /// <summary>
        ///     Register an implementation type.
        ///     The type is automatically registered.
        /// </summary>
        /// <typeparam name="TImplementer"></typeparam>
        /// <returns></returns>
        protected FluentRegistration<TImplementer> Register<TImplementer>()
        {
            // create the registration
            var registration = new FluentRegistration<TImplementer>();

            // save the registration
            _registrations.Add(registration);

            // return the registration
            return registration;
        }

        /// <summary>
        ///     Registers every public class from the assembly for the given type to (almost) every interface it implements.
        /// </summary>
        /// <remarks>
        ///     Note: Some system interfaces are excluded from registration. See the documentation for
        ///     TInjector.Initialization.UnlockedRegistration.AddAllServices.
        /// </remarks>
        /// <typeparam name="T">A type in the assembly containing all the types to register.</typeparam>
        protected void RegisterAll<T>()
        {
            RegisterAll<T>(null);
        }

        /// <summary>
        ///     Registers every public class from the assembly for the given type to (almost) every interface it implements.
        /// </summary>
        /// <remarks>
        ///     Note: Some system interfaces are excluded from registration. See the documentation for
        ///     TInjector.Initialization.UnlockedRegistration.AddAllServices.
        /// </remarks>
        /// <typeparam name="T">A type in the assembly containing all the types to register.</typeparam>
        /// <param name="inNamespace">
        ///     The namespace in which types will be registered. All types under this namespaces (and nested
        ///     namespaces) will be registered.
        /// </param>
        protected void RegisterAll<T>(string inNamespace)
        {
            // get all the public types form the assembly
            var registrations = typeof (T).Assembly.GetExportedTypes()
                // if there is a namespace filter apply it
                .Where(t => inNamespace == null || (t.Namespace != null && t.Namespace.StartsWith(inNamespace)))
                // make registrations for all the types
                .Select(t =>
                {
                    // make the registration
                    var registration = new UnlockedRegistration(t);

                    // register every service for the type
                    registration.AddAllServices();

                    // return the registration
                    return registration;
                });

            // add all the registrations
            foreach (var registration in registrations)
            {
                _registrations.Add(registration);
            }
        }
    }
}