﻿// TInjector: TInjector
// IRegistrationModule.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-17 8:28 PM

using System.Collections.Generic;

namespace TInjector.Registration
{
    /// <summary>
    ///     Functionality required by any module that produces registrations.
    /// </summary>
    public interface IRegistrationModule
    {
        IEnumerable<IRegistration> GenerateRegistrations();
    }
}