// TInjector: TInjector
// Root.cs
// Created: 2015-10-17 10:23 AM

using System;
using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector.Locator
{
    public class RootLocator : ILocator
    {
        private readonly IRegistrationProvider _registrations;
        private readonly IDictionary<Type, object> _singletonScopeCache;

        public RootLocator(params IRegistrationProvider[] registrationProviders)
        {
            if (registrationProviders == null)
            {
                throw new ArgumentNullException(nameof(registrationProviders));
            }

            _registrations = registrationProviders.Length == 1
                ? registrationProviders[0]
                : new AggregatedRegistrationProvider(registrationProviders);

            _singletonScopeCache = new Dictionary<Type, object>();
        }

        public object Get(Type service)
        {
            var locator = new GraphLocator(_registrations, _singletonScopeCache);
            var request = new Request(service, _registrations, locator);
            var result = locator.Get(request);
            return result;
        }

        public T Get<T>()
        {
            var locator = new GraphLocator(_registrations, _singletonScopeCache);
            var request = new Request(typeof(T), _registrations, locator);
            var result = locator.Get(request);
            return (T)result;
        }
    }
}