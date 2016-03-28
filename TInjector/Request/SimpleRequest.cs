using System;
using TInjector.Locator;
using TInjector.Registration;

namespace TInjector.Request
{
    public class SimpleRequest : IRequest
    {
        public ILocator Locator { get; private set; }

        public IRequest Parent { get; private set; }

        public Type Service { get; private set; }

        public IRegistrationCollection Registrations { get; private set; }

        public SimpleRequest(ILocator locator, IRegistrationCollection registrations, IRequest parent, Type service)
        {
            Locator = locator;
            Registrations = registrations;
            Parent = parent;
            Service = service;
        }
    }
}
