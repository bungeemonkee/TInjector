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
        private readonly IRegistrationCollection _registrations;
        private readonly IDictionary<Type, object> _singletonScopeCache;

        public RootLocator(params IRegistrationCollection[] registrationCollections)
        {
            if (registrationCollections == null)
            {
                throw new ArgumentNullException(nameof(registrationCollections));
            }

            _registrations = registrationCollections.Length == 1
                ? registrationCollections[0]
                : new RegistrationCollection(registrationCollections);

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