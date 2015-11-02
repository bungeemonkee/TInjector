// TInjector: TInjector
// IRegistration.cs
// Created: 2015-11-01 5:22 PM
// Modified: 2015-11-01 10:16 PM

using System;
using System.Collections.Generic;
using TInjector.Scope;

namespace TInjector.Registration
{
    /// <summary>
    ///     All functionality required for type registrations
    /// </summary>
    public interface IRegistration
    {
        Type Implementer { get; }
        string CreationStackTrace { get; }
        IEnumerable<Type> Services { get; }
        ScopeType Scope { get; }
    }
}