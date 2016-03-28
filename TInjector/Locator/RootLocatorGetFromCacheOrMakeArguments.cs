using System;
using System.Collections.Generic;
using TInjector.Registration;

namespace TInjector.Locator
{
    public class RootLocatorGetFromCacheOrMakeArguments : RootLocatorMakeArguments
    {
        public readonly IDictionary<Type, object> CacheToUse;

        public RootLocatorGetFromCacheOrMakeArguments(RootLocatorGetArguments copy, IRegistration registration, IDictionary<Type, object> cacheToUse)
            : base(copy, registration)
        {
            if (cacheToUse == null)
            {
                throw new ArgumentNullException(nameof(cacheToUse));
            }

            CacheToUse = cacheToUse;
        }
    }
}
