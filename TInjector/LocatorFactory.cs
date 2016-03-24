// TInjector: TInjector
// RootFactory.cs
// Created: 2015-10-17 10:23 AM

using System.Collections.Generic;
using System.Linq;
using TInjector.Registration;

namespace TInjector
{
    public class LocatorFactory : List<IRegistrationFactory>, ILocatorFactory
    {
        public LocatorFactory()
        { }

        public LocatorFactory(IEnumerable<IRegistrationFactory> registrationFactories)
            : base(registrationFactories)
        { }

        public ILocator GetLocator()
        {
            // get all the registrations by the service
            var registrations = this.SelectMany(x => x)
                .SelectMany(x => x.Services, (x, y) => new { Service = y, Registration = x })
                .GroupBy(x => x.Service)
                .ToDictionary(x => x.Key, x => x.Single().Registration);

            // create and return the new root
            return new Locator(registrations);
        }
    }
}