// TInjector: TInjector
// Root.cs
// Created: 2015-10-17 10:23 AM

using System;
using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector
{
    public class Locator : LocatorBase, ILocator
    {
        private readonly IReadOnlyDictionary<Type, IRegistration<object>> _registrationsByService;
        private readonly IDictionary<Type, object> _singletonScopeCache;

        public Locator(IReadOnlyDictionary<Type, IRegistration<object>> registrationsByService)
        {
            _registrationsByService = registrationsByService;
            _singletonScopeCache = new Dictionary<Type, object>();
        }

        public TService Get<TService>()
        {
            return (TService)Get(typeof(TService));
        }

        public object Get(Type service)
        {
            return Get(service, _registrationsByService, _singletonScopeCache, new Dictionary<Type, object>());
        }

        protected override Request MakeRequest(Type service, IRegistration<object> registration)
        {
            return new Request(service, _registrationsByService, _singletonScopeCache, registration);
        }

        public IRegistration<T> GetRegistration<T>() where T : class
        {
            IRegistration<object> result;
            _registrationsByService.TryGetValue(typeof(T), out result);
            return (IRegistration<T>)result;
        }

        public IRegistration<object> GetRegistration(Type service)
        {
            IRegistration<object> result;
            _registrationsByService.TryGetValue(service, out result);
            return result;
        }
    }
}