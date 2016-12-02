using System;
using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector.Locator
{
    /// <summary>
    /// Basically a locator but it only deals with <see cref="IRequest"/>s in the same object graph.
    /// </summary>
    public class GraphLocator
    {
        private readonly IRegistrationCollection _registrations;
        private readonly IDictionary<Type, object> _graphScopeCache;
        private readonly IDictionary<Type, object> _singletonScopeCache;

        public GraphLocator(IRegistrationCollection registrations, IDictionary<Type, object> singletonScopeCache)
        {
            _registrations = registrations;
            _singletonScopeCache = singletonScopeCache;
            _graphScopeCache = new Dictionary<Type, object>();
        }

        public object Get(IRequest request)
        {
            // get the registration
            var registration = _registrations.GetRegistration(request.Service);
            if (registration == null)
            {
                throw LocatorException.GetServiceNotRegisteredException(request.Service);
            }

            // check the scope to get the object from the right cache
            switch (registration.Scope)
            {
                case Scope.Singleton:
                    lock (_singletonScopeCache)
                    {
                        return GetFromCacheOrMake(request, registration, _singletonScopeCache);
                    }
                case Scope.Graph:
                    return GetFromCacheOrMake(request, registration, _graphScopeCache);

                case Scope.Transient:
                    return Make(request, registration);

                default:
                    throw LocatorException.GetInvalidScopeException(request.Service, registration.Scope);
            }
        }

        protected object GetFromCacheOrMake(IRequest request, IRegistration registration, IDictionary<Type, object> cache)
        {
            // get the result from the cache
            object result;
            if (cache.TryGetValue(registration.Implementer, out result)) return result;

            // create the result
            result = Make(request, registration);

            // put the result in the cache
            cache.Add(registration.Implementer, result);

            // return the result
            return result;
        }

        protected object Make(IRequest request, IRegistration registration)
        {
            // invoke the factory
            object result;
            try
            {
                result = registration.Factory.Make(request);
            }
            catch (Exception ex)
            {
                throw LocatorException.GetFactoryException(request, ex);
            }

            // Invoke all the activation callbacks in order
            foreach (var activator in registration.ActivationCallbacks)
            {
                // NOTE: Because of the way the implicit recursion in Make(...) works
                // these functions will be called with those for the lowest objects
                // in the graph first, slowly working up to the root object.
                try
                {
                    activator(request, result);
                }
                catch (Exception ex)
                {
                    throw LocatorException.GetActivationCallbackException(request, ex);
                }
            }

            // Return the object
            return result;
        }
    }
}
