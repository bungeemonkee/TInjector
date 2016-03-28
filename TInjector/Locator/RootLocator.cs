// TInjector: TInjector
// Root.cs
// Created: 2015-10-17 10:23 AM

using System;
using System.Collections.Generic;
using TInjector.Registration;
using TInjector.Request;

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
            var request = new SimpleRequest(this, _registrations, null, service);
            return Get(request);
        }

        public T Get<T>()
        {
            var request = new SimpleRequest<T>(this, _registrations, null);
            return (T)Get(request);
        }

        protected object Get(IRequest request)
        {
            var activationCallbacks = new List<Action>();
            var graphCache = new Dictionary<Type, object>();
            var args = new RootLocatorGetArguments(request, graphCache, activationCallbacks);
            var result = Get(args);

            return result;
        }

        protected object Get(RootLocatorGetArguments args)
        {
            // get the registration
            var registration = _registrations.GetRegistration(args.Request.Service);
            if (registration == null)
            {
                throw LocatorException.GetServiceNotRegisteredException(args.Request.Service);
            }

            // check the scope to get the object from the right cache
            switch (registration.Scope)
            {
                case Scope.Singleton:
                    var cacheArgs = new RootLocatorGetFromCacheOrMakeArguments(args, registration, _singletonScopeCache);
                    lock (_singletonScopeCache)
                    {
                        return GetFromCacheOrMake(cacheArgs);
                    }

                case Scope.Graph:
                    var cacheArgs2 = new RootLocatorGetFromCacheOrMakeArguments(args, registration, args.GraphCache);
                    return GetFromCacheOrMake(cacheArgs2);

                case Scope.Transient:
                    var makeArgs = new RootLocatorMakeArguments(args, registration);
                    return Make(makeArgs);

                default:
                    throw LocatorException.GetInvalidScopeException(args.Request.Service, registration.Scope);
            }
        }

        protected object GetFromCacheOrMake(RootLocatorGetFromCacheOrMakeArguments args)
        {
            // get the result from the cache
            object result;
            if (args.CacheToUse.TryGetValue(args.Registration.Implementer, out result)) return result;

            // create the result
            result = Make(args);

            // put the result in the cache
            args.CacheToUse.Add(args.Registration.Implementer, result);

            // return the result
            return result;
        }

        protected object Make(RootLocatorMakeArguments args)
        {
            // create the child locator
            var locator = new GraphLocator(args.Request, _registrations, r => Get(new RootLocatorGetArguments(r, args.GraphCache, args.ActivationCallbacks)));

            // invoke the factory
            object result;
            try
            {
                result = args.Registration.Factory.Make(args.Request);
            }
            catch (Exception ex)
            {
                throw LocatorException.GetFactoryException(args.Request, ex);
            }

            // Add the activator functions to the list
            // These will be executed after the entire object graph is built
            foreach (var activator in args.Registration.ActivationCallbacks)
            {
                args.ActivationCallbacks.Add(() => activator(args.Request, result));
            }

            // Return the object
            return result;
        }

        protected void InvokeActivationCallbacks(RootLocatorGetArguments args)
        {
            // Invoke all the activation callbacks in order
            // NOTE: Because of the way the implicit recursion in Make(...) works
            // these functions will be called with those for the lowest objects
            // in the graph first, slowly working up to the root object.
            try
            {
                foreach (var action in args.ActivationCallbacks)
                {
                    action();
                }
            }
            catch (Exception ex)
            {
                throw LocatorException.GetActivationCallbackException(args.Request, ex);
            }
        }
    }
}