// TInjector: TInjector
// Root.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-18 11:32 AM

using System;
using System.Collections.Generic;
using TInjector.Build;
using TInjector.Localization;

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
            // make sure we can resolve the request
            if (!_buildersByService.ContainsKey(service))
            {
                throw new InvalidOperationException(string.Format(Resources.TInjector_Root_UnregisteredService, service.FullName));
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