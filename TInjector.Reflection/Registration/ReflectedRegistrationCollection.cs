using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TInjector.Registration;

namespace TInjector.Reflection.Registration
{
    public class ReflectedRegistrationCollection : IRegistrationProvider
    {
        private static readonly Type ReflectedRegistrationType = typeof(ReflectedRegistration<>);
        private static readonly Type GenericRegistrationType = typeof(GenericRegistration<,>);

        private readonly IDictionary<Type, IRegistration> _registrations;
        private readonly IDictionary<Type, GenericParameterMap> _genericsByInterfaces;

        public static ReflectedRegistrationCollection ForAssemblyOfType<T>()
        {
            return new ReflectedRegistrationCollection(typeof(T).Assembly);
        }

        public ReflectedRegistrationCollection(params Assembly[] assemblies)
        {
            // Get registrations for every non-generic type in the assemblies
            _registrations = assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsGenericTypeDefinition)
                .Select(x => ReflectedRegistrationType.MakeGenericType(x))
                .Select(x => (IRegistration)Activator.CreateInstance(x))
                .SelectMany(x => x.Services, (x, y) => new
                {
                    Service = y,
                    Registration = x,
                })
                .ToDictionary(x => x.Service, x => x.Registration);

            // Get all the generic types in the assembly by the interfaces they implement
            // Only get interfaces that have the same number of generic parameters
            _genericsByInterfaces = assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .Where(x => x.IsGenericTypeDefinition)
                .SelectMany(x => x.GetInterfaces()
                    .Where(y => y.IsGenericType)
                    .Select(y => new
                    {
                        Generic = GenericParameterMap.Create(y, x),
                        Service = y.GetGenericTypeDefinition(),
                    }))
                .Where(x => x != null)
                .ToDictionary(x => x.Service, x => x.Generic);
        }

        public IRegistration GetRegistration(Type service)
        {
            IRegistration result;
            if (_registrations.TryGetValue(service, out result)) return result;

            if (!service.IsGenericType) return null;

            lock (_registrations)
            {
                // Make sure the registration wasn't made by another thread while waiting for the lock
                if (_registrations.TryGetValue(service, out result)) return result;

                var interfaceDefinition = service.GetGenericTypeDefinition();

                GenericParameterMap implementorDefinition;
                if (!_genericsByInterfaces.TryGetValue(interfaceDefinition, out implementorDefinition)) return null;

                var parameters = implementorDefinition.RearrangeParameters(service.GetGenericArguments());
                var implementor = implementorDefinition.Implementor.MakeGenericType(parameters);
                var registrationType = GenericRegistrationType.MakeGenericType(service, implementor);
                result = (IRegistration)Activator.CreateInstance(registrationType);

                _registrations.Add(service, result);
            }

            return result;
        }
    }
}
