using System;
using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector
{
    public class Request : LocatorBase, IRequest
    {
        private readonly IReadOnlyDictionary<Type, IRegistration<object>> _registrationsByService;
        private readonly IDictionary<Type, object> _singletonScopeCache;
        private readonly IDictionary<Type, object> _graphScopeCache;
        private readonly IRegistration<object> _requestRegistration;
        private readonly Request _parentRequest;

        protected IReadOnlyDictionary<Type, IRegistration<object>> RegistrationsByService => _registrationsByService ?? _parentRequest.RegistrationsByService;

        protected IDictionary<Type, object> SingletonScopeCache => _singletonScopeCache ?? _parentRequest.SingletonScopeCache;

        protected IDictionary<Type, object> GraphScopeCache => _graphScopeCache ?? _parentRequest.GraphScopeCache;

        public IRequest Parent => _parentRequest;

        public Type Service { get; private set; }

        public Type Implementer => _requestRegistration.Implementer;

        public Scope Scope => _requestRegistration.Scope;

        public Request(Type service, IReadOnlyDictionary<Type, IRegistration<object>> registrationsByService, IDictionary<Type, object> singletonScopeCache, IRegistration<object> requestRegistration)
        {
            Service = service;
            _registrationsByService = registrationsByService;
            _singletonScopeCache = singletonScopeCache;
            _requestRegistration = requestRegistration;
            _graphScopeCache = new Dictionary<Type, object>();
        }

        protected Request(Type service, Request parentRequest, IRegistration<object> requestRegistration)
        {
            Service = service;
            _parentRequest = parentRequest;
            _requestRegistration = requestRegistration;
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public object Get(Type service)
        {
            return Get(service, RegistrationsByService, SingletonScopeCache, GraphScopeCache);
        }

        protected override Request MakeRequest(Type service, IRegistration<object> registration)
        {
            return new Request(service, this, registration);
        }

        public IRegistration<T> GetRegistration<T>() where T : class
        {
            IRegistration<object> result;
            RegistrationsByService.TryGetValue(typeof(T), out result);
            return (IRegistration<T>)result;
        }

        public IRegistration<object> GetRegistration(Type service)
        {
            IRegistration<object> result;
            RegistrationsByService.TryGetValue(service, out result);
            return result;
        }
    }
}
