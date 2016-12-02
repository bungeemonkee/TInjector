using System;
using TInjector.Registration;

namespace TInjector.Locator
{
    public class Request : IRequest
    {
        private readonly GraphLocator _innerLocator;

        public IRequest Parent { get; }

        public Type Service { get; }

        public IRegistrationProvider Registrations { get; }

        public Request(Type service, IRegistrationProvider registrations, GraphLocator innerLocator)
        {
            Service = service;
            Registrations = registrations;
            _innerLocator = innerLocator;
        }

        private Request(Type service, Request parent)
        {
            Service = service;
            Parent = parent;
            Registrations = parent.Registrations;
            _innerLocator = parent._innerLocator;
        }

        public T Get<T>()
        {
            var request = new Request(typeof(T), this);
            var result = _innerLocator.Get(request);
            return (T)result;
        }

        public object Get(Type service)
        {
            var request = new Request(service, this);
            var result = _innerLocator.Get(request);
            return result;
        }
    }
}
