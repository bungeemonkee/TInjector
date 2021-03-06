﻿// TInjector: TInjector.Web.Mvc
// TInjectorMvcApplication.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-18 11:32 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TInjector.Locator;
using TInjector.Registration;

namespace TInjector.Web.Mvc
{
    public abstract class TInjectorMvcApplication : HttpApplication
    {
        protected static ILocator Locator { get; private set; }

        protected abstract IEnumerable<IRegistrationProvider> GetRegistrationCollections();

        public virtual void Application_Start(object sender, EventArgs e)
        {
            // Note: The HttpApplication constructor may be called more than once in the same application domain.
            // Since we only want a single root factory (and root) we need to use Application_Start when generating the root.

            // if there is already a root then return, shouldn't happen but it's worth it to be sure
            if (Locator != null) return;

            // construct the root singleton
            Locator = new RootLocator(GetRegistrationCollections().ToArray());

            // create the new dependency resolver
            var dependencyResolver = new TInjectorDependencyResolver(Locator);

            // set the new dependency resolver
            DependencyResolver.SetResolver(dependencyResolver);
        }
    }
}