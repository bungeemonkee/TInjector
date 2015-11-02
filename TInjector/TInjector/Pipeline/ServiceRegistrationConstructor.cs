// TInjector: TInjector
// ServiceRegistrationConstructor.cs
// Created: 2015-11-01 5:35 PM

using System;
using System.Reflection;

namespace TInjector.Pipeline
{
    public class ServiceRegistrationConstructor : ServiceRegistration
    {
        public readonly ConstructorInfo Constructor;
        public readonly Type[] ConstructorParameters;

        public ServiceRegistrationConstructor(ServiceRegistration service, ConstructorInfo constructor, Type[] constructorParameters)
            : base(service.Service, service.Registration)
        {
            Constructor = constructor;
            ConstructorParameters = constructorParameters;
        }
    }
}