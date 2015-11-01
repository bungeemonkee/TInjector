// TInjector: TInjector
// IConstructorSelector.cs
// Created: 2015-11-01 5:30 PM
// Modified: 2015-11-01 6:31 PM

using System;
using System.Linq;

namespace TInjector.Pipeline
{
    public interface IConstructorSelector
    {
        ILookup<Type, ServiceRegistrationConstructor> Execute(ILookup<Type, ServiceRegistration> services);
    }
}