// TInjector: TInjector
// UnlockedRegistration.cs
// Created: 2015-10-17 7:37 PM

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TInjector.Scope;

namespace TInjector.Registration
{
    /// <summary>
    ///     A registration which can be changed.
    /// </summary>
    public class UnlockedRegistration : IRegistration
    {
        public UnlockedRegistration(Type implementer)
        {
            // validate the implementer type
            ValidateImplementerType(implementer);

            // save the properties
            Implementer = implementer;
            CreationStackTrace = new StackTrace().ToString();
            Services = new HashSet<Type>();
        }

        public Type Implementer { get; private set; }
        public string CreationStackTrace { get; private set; }
        public IEnumerable<Type> Services { get; private set; }
        public ScopeType Scope { get; protected set; }

        /// <summary>
        ///     Registers an implementer type against the given service type.
        /// </summary>
        /// <param name="service">The type of the service as which the implementer will be registered.</param>
        public void AddService(Type service)
        {
            // validate the service type
            ValidateServiceType(service);

            // get the implemented services (as the underlying set type)
            var implemented = (HashSet<Type>) Services;

            // if the service is not already registered...
            if (!implemented.Contains(service))
            {
                // add the service
                implemented.Add(service);
            }
        }

        /// <summary>
        ///     Registers an implementation type against (almost) all interfaces it implements.
        /// </summary>
        /// <remarks>
        ///     The following interfaces are ignored if they are implemented:
        ///     * System.IDisposable
        ///     * System.ComponentModel.IComponent
        ///     * System.ComponentModel.IContainer
        /// </remarks>
        public void AddAllServices()
        {
            // the interface types to ignore
            var ignore = new[]
            {
                typeof (IDisposable),
                typeof (IComponent),
                typeof (IContainer)
            };

            // get all the services to automatically register
            var services = Implementer.GetInterfaces()
                // ignore explicitly ignored services
                .Where(i => !ignore.Contains(i))
                // ignore services already registered
                .Except(Services);

            // get the implemented services (as the underlying set type)
            var implemented = (HashSet<Type>) Services;

            // for each service...
            foreach (var service in services)
            {
                // add the service
                implemented.Add(service);
            }
        }

        private static void ValidateImplementerType(Type implementer)
        {
            if (implementer.IsInterface)
            {
                throw new InvalidOperationException(string.Format("Unable to register {0} because it is an interface.", implementer.FullName));
            }

            if (implementer.IsAbstract)
            {
                throw new InvalidOperationException(string.Format("Unable to register {0} because it is abstract.", implementer.FullName));
            }
        }

        private void ValidateServiceType(Type service)
        {
            if (!service.IsAssignableFrom(Implementer))
            {
                throw new InvalidOperationException(string.Format("Unable to register {0} as {1} because {0} can not be cast to { 1}.", Implementer.FullName, service.FullName));
            }
        }
    }
}