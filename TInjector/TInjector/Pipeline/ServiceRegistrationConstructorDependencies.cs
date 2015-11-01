// TInjector: TInjector
// ServiceRegistrationConstructorDependencies.cs
// Created: 2015-11-01 6:09 PM
// Modified: 2015-11-01 6:31 PM

namespace TInjector.Pipeline
{
    public class ServiceRegistrationConstructorDependencies : ServiceRegistrationConstructor
    {
        public readonly ServiceRegistrationConstructor[] Dependencies;

        public ServiceRegistrationConstructorDependencies(ServiceRegistrationConstructor service, ServiceRegistrationConstructor[] dependencies)
            : base(service, service.Constructor, service.ConstructorParameters)
        {
            Dependencies = dependencies;
        }
    }
}