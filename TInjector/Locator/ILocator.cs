// TInjector: TInjector
// IRoot.cs
// Created: 2015-10-17 10:23 AM

using System;
using TInjector.Registration;

namespace TInjector.Locator
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
    }
}