// TInjector: TInjector
// Builder.cs
// Created: 2015-11-01 9:42 PM

using System;
using System.Collections.Generic;
using System.Linq;
using TInjector.Registration;

namespace TInjector.Pipeline
{
    public class Builder : IBuilder
    {
        public Builder(Type serviceType, IEnumerable<ServiceRegistrationConstructorDependencies> registrations)
        {
            ServiceType = serviceType;
            Registrations = registrations.Select(r => r.Registration).ToArray();
        }

        public Type ServiceType { get; private set; }
        public IRegistration[] Registrations { get; private set; }
        public IBuilder[] Dependencies { get; private set; }

        public object GetOrCreateInstance(IDictionary<Type, object> singletonCache, IDictionary<Type, object> perGraphCache)
        {
            throw new NotImplementedException();
        }
    }
}