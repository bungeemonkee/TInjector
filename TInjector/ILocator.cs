// TInjector: TInjector
// IRoot.cs
// Created: 2015-10-17 10:23 AM

using System;
using TInjector.Registration;

namespace TInjector
{
    /// <summary>
    ///     Functionality all DI root objects must implement.
    ///     Note: Implementers must be thread-safe.
    /// </summary>
    public interface ILocator
    {
        /// <summary>
        ///     Get a service implementation (strongly typed).
        /// </summary>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        /// <returns>An implementation of the given service type.</returns>
        T Get<T>();

        /// <summary>
        ///     Get a service implementation (as an object).
        ///     You will have to cast the object back to the type you want.
        ///     It is *HIGHLY* recommended to use the strongly typed Get&lt;T&gt;() version whenever possible.
        /// </summary>
        /// <param name="service">The type of the service to get.</param>
        /// <returns>An implementation of the given service type.</returns>
        object Get(Type service);

        /// <summary>
        ///     Gets the registration for a given service.
        /// </summary>
        /// <returns>The registration for the given service or null if none exists.</returns>
        IRegistration<T> GetRegistration<T>() where T : class;

        /// <summary>
        ///     Gets the registration for a given service.
        ///     The returned registration object can be casted up to IRegistration&lt;T&gt; where T is the type of service.
        ///     It is *HIGHLY* recommended to use the strongly typed GetRegistration&lt;T&gt;() version whenever possible.
        /// </summary>
        /// <param name="service">The service type to get the registration for.</param>
        /// <returns>The registration for the given service or null if none exists.</returns>
        IRegistration<object> GetRegistration(Type service);
    }
}