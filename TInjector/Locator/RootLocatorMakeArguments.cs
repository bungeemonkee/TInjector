using System;
using TInjector.Registration;

namespace TInjector.Locator
{
    public class RootLocatorMakeArguments : RootLocatorGetArguments
    {
        /// <summary>
        /// The registration for this request.
        /// </summary>
        public readonly IRegistration Registration;

        public RootLocatorMakeArguments(RootLocatorGetArguments copy, IRegistration registration)
            : base (copy)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }
        }
    }
}
