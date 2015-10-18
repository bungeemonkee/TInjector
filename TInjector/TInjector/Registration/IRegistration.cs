// TInjector: TInjector
// IRegistration.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-18 11:32 AM

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