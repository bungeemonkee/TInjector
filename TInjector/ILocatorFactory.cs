// TInjector: TInjector
// IRootFactory.cs
// Created: 2015-10-17 10:23 AM

using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector
{
    /// <summary>
    ///     Functionality required for all locator factory objects.
    /// </summary>
    public interface ILocatorFactory : IList<IRegistrationFactory>
    {
        /// <summary>
        ///     Get a <see cref="ILocator"/> from this factory.
        /// </summary>
        /// <returns></returns>
        ILocator GetLocator();
    }
}