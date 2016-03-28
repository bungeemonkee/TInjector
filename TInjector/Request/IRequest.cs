using System;
using TInjector.Locator;
using TInjector.Registration;

namespace TInjector
{
    public interface IRequest
    {
        IRequest Parent { get; }

        Type Service { get; }

        ILocator Locator { get; }

        IRegistrationCollection Registrations { get; }
}
}
