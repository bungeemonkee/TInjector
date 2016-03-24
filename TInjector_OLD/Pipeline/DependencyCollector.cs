// TInjector: TInjector
// DependencyCollector.cs
// Created: 2015-11-01 6:13 PM

using System;
using System.Linq;

namespace TInjector.Pipeline
{
    public class DependencyCollector : IDependencyCollector
    {
        public ILookup<Type, ServiceRegistrationConstructorDependencies> Execute(ILookup<Type, ServiceRegistrationConstructor> services)
        {
            return services.SelectMany(s => s)
                .Select(s => CollectDependencies(services, s))
                .ToLookup(s => s.Service, s => s);
        }

        public ServiceRegistrationConstructorDependencies CollectDependencies(ILookup<Type, ServiceRegistrationConstructor> services, ServiceRegistrationConstructor service)
        {
            if (service.ConstructorParameters == null) return null;

            var dependencies = new ServiceRegistrationConstructor[service.ConstructorParameters.Length];

            for (var i = 0; i < dependencies.Length; ++i)
            {
                dependencies[i] = services[service.ConstructorParameters[i]].First();
            }

            return new ServiceRegistrationConstructorDependencies(service, dependencies);
        }
    }
}