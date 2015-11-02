// TInjector: TInjector
// IBuilderGenerator.cs
// Created: 2015-11-01 10:04 PM
// Modified: 2015-11-01 10:16 PM

using System;
using System.Collections.Generic;
using System.Linq;

namespace TInjector.Pipeline
{
    public interface IBuilderGenerator
    {
        IDictionary<Type, IBuilder> Execute(ILookup<Type, ServiceRegistrationConstructorDependencies> services);
    }
}