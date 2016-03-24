// TInjector: TInjector
// IRegistrationGenerator.cs
// Created: 2015-11-01 5:24 PM

using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector.Pipeline
{
    /// <summary>
    ///     Pipeline Step 1 - Generating registrations.
    /// </summary>
    public interface IRegistrationGenerator
    {
        /// <summary>
        ///     Generates registrations.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IRegistration> Execute();
    }
}