using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TInjector.Registration;

namespace TInjector.Reflection.Registration
{
    public class ReflectedRegistrationCollection : IRegistrationCollection
    {
        private static readonly Type RegistrationType = typeof(ReflectedRegistration<>);

        private readonly IDictionary<Type, IRegistration> _registrations;

        public static ReflectedRegistrationCollection ForAssemblyOfType<T>()
        {
            return new ReflectedRegistrationCollection(typeof(T).Assembly);
        }

        public ReflectedRegistrationCollection(params Assembly[] assemblies)
        {
            _registrations = assemblies
                .Select(x => x.GetExportedTypes())
                .SelectMany(x => x)
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsGenericTypeDefinition) // TODO: Support generics?
                .Select(x => RegistrationType.MakeGenericType(x))
                .Select(x => (IRegistration)Activator.CreateInstance(x))
                .SelectMany(x => x.Services, (x, y) => new
                {
                    Service = y,
                    Registration = x
                })
                .ToDictionary(x => x.Service, x => x.Registration);
        }

        public IEnumerator<IRegistration> GetEnumerator()
        {
            return _registrations.Values.GetEnumerator();
        }

        public IRegistration GetRegistration(Type service)
        {
            return _registrations[service];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
