using System;
using TInjector.Registration;
using TInjector.Request;

namespace TInjector.Locator
{
    public class GraphLocator : ILocator
    {
        private readonly IRequest _parentRequest;
        private readonly IRegistrationCollection _registrations;
        private readonly Func<IRequest, object> _get;

        public GraphLocator(IRequest parentRequest, IRegistrationCollection registrations, Func<IRequest, object> get)
        {
            _parentRequest = parentRequest;
            _registrations = registrations;
            _get = get;
        }

        public object Get(Type service)
        {
            var request = new SimpleRequest(this, _registrations, _parentRequest, service);
            return _get(request);
        }

        public T Get<T>()
        {
            var request = new SimpleRequest<T>(this, _registrations, _parentRequest);
            return (T)_get(request);
        }
    }
}
