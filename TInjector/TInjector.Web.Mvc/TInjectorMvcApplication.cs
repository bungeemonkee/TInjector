// TInjector: TInjector.Web.Mvc
// TInjectorMvcApplication.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-18 11:32 AM

using System;
using System.Web;
using System.Web.Mvc;

namespace TInjector.Web.Mvc
{
    public abstract class TInjectorMvcApplication : HttpApplication
    {
        protected static IRoot Root { get; private set; }

        protected abstract IRootFactory GetRootFactory();

        public virtual void Application_Start(object sender, EventArgs e)
        {
            // Note: The HttpApplication constructor may be called more than once in the same application domain.
            // Since we only want a single root factory (and root) we need to lock when generating the root.

            // if there is already a root then return, shouldn't happen but it's worth it to be sure
            if (Root != null) return;

            // construct the root singleton
            Root = GetRootFactory().GetRoot();

            // create the new dependency resolver
            var dependencyResolver = new TInjectorDependencyResolver(Root);

            // set the new dependency resolver
            DependencyResolver.SetResolver(dependencyResolver);
        }
    }
}