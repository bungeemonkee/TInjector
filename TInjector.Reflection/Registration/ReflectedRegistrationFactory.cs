using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TInjector.Registration;

namespace TInjector.Reflection.Registration
{
    public class ReflectedRegistrationFactory : IRegistrationFactory
    {
        private static readonly Type RegistrationType = typeof(ReflectedRegistration<>);

        private readonly Assembly _assembly;

        public ReflectedRegistrationFactory(Assembly assembly)
        {
            _assembly = assembly;
        }

        public static ReflectedRegistrationFactory ForAssemblyOfType<T>()
        {
            return new ReflectedRegistrationFactory(typeof(T).Assembly);
        }

        public IEnumerator<IRegistration<object>> GetEnumerator()
        {
            return _assembly
                .GetExportedTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsClass)
                .Where(x => !x.IsGenericTypeDefinition) // TODO: Support generics?
                .Select(x => RegistrationType.MakeGenericType(x))
                .Select(x => (IRegistration<object>)Activator.CreateInstance(x))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
