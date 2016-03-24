// TInjector: TInjector
// IDependencyCollector.cs
// Created: 2015-11-01 6:12 PM

using System;
using System.Linq;

namespace TInjector.Pipeline
{
    public interface IDependencyCollector
    {
        ILookup<Type, ServiceRegistrationConstructorDependencies> Execute(ILookup<Type, ServiceRegistrationConstructor> services);
    }
}