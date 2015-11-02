// TInjector: TInjector
// LockedRegistration.cs
// Created: 2015-10-17 7:26 PM

using System;
using System.Collections.Generic;
using System.Linq;
using TInjector.Scope;

namespace TInjector.Registration
{
    /// <summary>
    ///     A registration which can not be changed.
    /// </summary>
    public class LockedRegistration : IRegistration
    {
        /// <summary>
        ///     Produces a LockedRegistration with the same properties as the given registration.
        /// </summary>
        /// <param name="registration"></param>
        public LockedRegistration(IRegistration registration)
        {
            Implementer = registration.Implementer;
            CreationStackTrace = registration.CreationStackTrace;
            Services = registration.Services.ToArray();
            Scope = registration.Scope;
        }

        public Type Implementer { get; private set; }
        public string CreationStackTrace { get; private set; }
        public IEnumerable<Type> Services { get; private set; }
        public ScopeType Scope { get; private set; }
    }
}