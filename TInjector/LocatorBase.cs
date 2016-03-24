using System;
using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector
{
    public abstract class LocatorBase
    {
        protected abstract Request MakeRequest(Type service, IRegistration<object> registration);

        protected object Get(Type service, IReadOnlyDictionary<Type, IRegistration<object>> registrationsByService, IDictionary<Type, object> singletonScopeCache, IDictionary<Type, object> graphScopeCache)
        {
            // make sure we can resolve the request
            IRegistration<object> registration;
            if (!registrationsByService.TryGetValue(service, out registration))
            {
                throw RequestException.GetServiceNotRegisteredException(service);
            }

            // check the scope to get the object from the right cache
            switch (registration.Scope)
            {
                case Scope.Singleton:
                    lock (singletonScopeCache)
                    {
                        return GetFromCacheOrMake(service, singletonScopeCache, registration);
                    }
                case Scope.Graph:
                    return GetFromCacheOrMake(service, graphScopeCache, registration);
                case Scope.Transient:
                    return Make(service, registration);
                default:
                    throw RequestException.GetInvalidScopeException(service, registration.Scope);
            }
        }

        private object GetFromCacheOrMake(Type service, IDictionary<Type, object> cache, IRegistration<object> registration)
        {
            // get the result from the cache
            object result;
            if (cache.TryGetValue(registration.Implementer, out result)) return result;

            // create the result
            result = Make(service, registration);

            // put the result in the cache
            cache.Add(registration.Implementer, result);

            // return the result
            return result;
        }

        private object Make(Type service, IRegistration<object> registration)
        {
            // create the child request
            var request = MakeRequest(service, registration);

            // invoke the factory
            try
            {
                return registration.Factory.Make(request);
            }
            catch (Exception ex)
            {
                throw RequestException.GetFactoryException(request, ex);
            }
        }
    }
}
