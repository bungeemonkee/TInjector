// TInjector: TInjector
// IDependencyCollector.cs
// Created: 2015-11-01 6:12 PM
// Modified: 2015-11-01 6:31 PM

using System;
using System.Linq;

namespace TInjector.Pipeline
{
    public interface IDependencyCollector
    {
        ILookup<Type, ServiceRegistrationConstructorDependencies> Execute(ILookup<Type, ServiceRegistrationConstructor> services);
    }
}