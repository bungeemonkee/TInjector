using System;
using TInjector.Registration;

namespace TInjector.Locator
{
    public interface IRequest : ILocator
    {
        IRequest Parent { get; }

        Type Service { get; }

        IRegistrationProvider Registrations { get; }
    }
}
