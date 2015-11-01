// TInjector: TInjector
// IValidator.cs
// Created: 2015-11-01 5:24 PM
// Modified: 2015-11-01 6:31 PM

using System;
using System.Linq;

namespace TInjector.Pipeline
{
    public interface IValidator
    {
        void Execute(ILookup<Type, ServiceRegistrationConstructorDependencies> services);
    }
}