// TInjector: TInjector
// Root.cs
// Created: 2015-10-17 10:23 AM

using System;
using System.Collections.Generic;
using TInjector.Pipeline;

namespace TInjector
{
    public class Root : IRoot
    {
        private readonly IDictionary<Type, IBuilder> _buildersByService;
        private readonly IDictionary<Type, object> _singletonScopeCache;

        public Root(IDictionary<Type, IBuilder> buildersByService)
        {
            _singletonScopeCache = new Dictionary<Type, object>();
            _buildersByService = buildersByService;
        }

        public TService Get<TService>()
        {
            return (TService) Get(typeof (TService));
        }

        public object Get(Type service)
        {
            const string format = @"Unable to resolve request for service { 0} as no such service is registered.";

            // make sure we can resolve the request
            if (!_buildersByService.ContainsKey(service))
            {
                throw new InvalidOperationException(string.Format(format, service.FullName));
            }

            // create the scope cache for this object graph
            var graphScopeCache = new Dictionary<Type, object>();

            // get the builder
            var builder = _buildersByService[service];

            // build and return the object
            return builder.GetOrCreateInstance(_singletonScopeCache, graphScopeCache);
        }
    }
}