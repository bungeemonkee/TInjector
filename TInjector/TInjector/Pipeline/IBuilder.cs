// TInjector: TInjector
// IBuilder.cs
// Created: 2015-11-01 9:42 PM
// Modified: 2015-11-01 10:16 PM

using System;
using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector.Pipeline
{
    /// <summary>
    ///     Everything necessary to build an instance.
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        ///     The builder service type.
        ///     This can be different from the service type for the registration in the case where there are multiple registrations
        ///     for a single service. Then this would be an array type.
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        ///     The registrations for the object(s) that this builder creates.
        /// </summary>
        IRegistration[] Registrations { get; }

        /// <summary>
        ///     Builders for any dependencies this builder has.
        /// </summary>
        IBuilder[] Dependencies { get; }

        object GetOrCreateInstance(IDictionary<Type, object> singletonCache, IDictionary<Type, object> perGraphCache);
    }
}