// TInjector: TInjector.Web.Mvc
// TInjectorDependencyResolver.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-18 11:32 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TInjector.Locator;

namespace TInjector.Web.Mvc
{
    /// <summary>
    ///     An MVC IDependencyResolver that
    /// </summary>
    public class TInjectorDependencyResolver : IDependencyResolver
    {
        private readonly ILocator _locator;
        private readonly IDependencyResolver _secondaryResolver;

        public TInjectorDependencyResolver(ILocator locator)
            : this(locator, DependencyResolver.Current)
        {
        }

        public TInjectorDependencyResolver(ILocator locator, IDependencyResolver secondaryResolver)
        {
            _locator = locator;
            _secondaryResolver = secondaryResolver;
        }

        public object GetService(Type serviceType)
        {
            // get the object
            return Get<IEnumerable<object>>(serviceType, serviceType).First();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            // TODO: Support actual enumerables

            try
            {
                // get the service from the locator
                return Enumerable.Repeat(_locator.Get(serviceType), 1);
            }
            catch
            {
                // if there is a secondary dependency resolver...
                // get the service from the secondary dependency resolver
                var result = _secondaryResolver?.GetServices(serviceType);

                // if the secondary dependency resolver created anything...
                if (result != null)
                {
                    // return the secondary resolver's result
                    return result;
                }

                // there is no secondary resolver or it couldn't find anything
                throw;
            }
        }

        private T Get<T>(Type rootType, Type serviceType)
        {
            try
            {
                // get the service from the locator
                return (T)_locator.Get(rootType);
            }
            catch
            {
                // if there is a secondary dependency resolver...
                // get the service from the secondary dependency resolver
                var result = _secondaryResolver?.GetService(serviceType);

                // if the secondary dependency resolver created anything...
                if (result != null)
                {
                    // return the secondary resolver's result
                    return (T)result;
                }

                // there is no secondary resolver or it couldn't find anything
                throw;
            }
        }
    }
}