// TInjector: TInjector.Web.Mvc
// TInjectorDependencyResolver.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-17 8:28 PM

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TInjector.Web.Mvc
{
    /// <summary>
    ///     An MVC IDependencyResolver that
    /// </summary>
    public class TInjectorDependencyResolver : IDependencyResolver
    {
        private readonly IRoot _root;
        private readonly IDependencyResolver _secondaryResolver;

        public TInjectorDependencyResolver(IRoot root)
            : this(root, DependencyResolver.Current)
        {
        }

        public TInjectorDependencyResolver(IRoot root, IDependencyResolver secondaryResolver)
        {
            _root = root;
            _secondaryResolver = secondaryResolver;
        }

        public object GetService(Type serviceType)
        {
            // get the object
            return Get<IEnumerable<object>>(serviceType, serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            // construct the enumerable type we need to pass to the root
            // TODO: Reflection - see if we can optimize or work around this
            var enumerableType = typeof (IEnumerable<>).MakeGenericType(serviceType);

            // get the object
            return Get<IEnumerable<object>>(enumerableType, serviceType);
        }

        private T Get<T>(Type rootType, Type serviceType)
        {
            try
            {
                // get the service from the root
                return (T) _root.Get(rootType);
            }
            catch
            {
                // if there is a secondary dependency resolver...
                if (_secondaryResolver != null)
                {
                    // get the service from the secondary dependency resolver
                    var result = _secondaryResolver.GetService(serviceType);

                    // if the secondary dependency resolver created anything...
                    if (result != null)
                    {
                        // return the secondary resolver's result
                        return (T) result;
                    }
                }

                // there is no secondary resolver or it couldn't find anything
                throw;
            }
        }
    }
}