// TInjector: TInjector
// RootFactory.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-18 11:32 AM

using System;
using System.Collections.Generic;
using System.Linq;
using TInjector.Build;
using TInjector.Localization;
using TInjector.Registration;

namespace TInjector
{
    public class RootFactory : List<IRegistrationModule>, IRootFactory
    {
        // the collection types we can safely map to our array builders
        private static readonly Type[] EnumerableTypes =
        {
            typeof (IEnumerable<>),
            typeof (ICollection<>),
            typeof (IList<>)
        };

        public IRoot GetRoot()
        {
            // get all the registrations and lock them
            // TODO: We may be able to save time for large collections of modules if we run GenerateRegistrations in multiple threads at once
            var lockedRegistrations = this
                .SelectMany(m => m.GenerateRegistrations())
                .Select(r => new LockedRegistration(r))
                .ToArray();

            // make sure no implementer is registered more than once
            ValidateDuplicateImplementerRegistrations(lockedRegistrations);

            // TODO: Check for duplicated service registrations where there is no default selected
            // TODO: Should this even be an error?
            // TODO: Or do we only support is at a list?
            // TODO: Should we allow default services for list of services?
            // TODO: Should requesting a single instance of a duplicated service be an error?

            // convert the registrations to a lookup by service type
            var registrationsByService = lockedRegistrations
                .SelectMany(r => r.Services, (r, s) => new {Service = s, Registration = r})
                .ToLookup(r => r.Service, r => (IRegistration) r.Registration);

            // create a builder for each service
            // TODO: We may be able to save time for large collections of registrations if we create the builders in multiple threads at once
            var buildersByService = registrationsByService
                // create a builder for this service
                .Select(l => new Builder(l.Key, registrationsByService))
                // for services that are arrays also support several collections
                .SelectMany(b => b.ServiceType.IsArray
                    // add the mappings for the collection types, and keep the mapping for the original array
                    ? EnumerableTypes.Select(t => new KeyValuePair<Type, IBuilder>(t.MakeGenericType(b.ServiceType.GetElementType()), b))
                        .Union(new[] {new KeyValuePair<Type, IBuilder>(b.ServiceType, b)})
                    // keep the mapping for the original service type
                    : new[] {new KeyValuePair<Type, IBuilder>(b.ServiceType, b)})
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // create and return the new root
            return new Root(buildersByService);
        }

        public void Register(params IRegistrationModule[] modules)
        {
            // Just save the registration, no validation yet - just wait to build the container
            foreach (var module in modules)
            {
                Add(module);
            }
        }

        private static void ValidateDuplicateImplementerRegistrations(IEnumerable<IRegistration> registrations)
        {
            // get all the registrations grouped by implementer
            var duplicatedImplementationRegistrations = registrations
                .GroupBy(r => r.Implementer)
                .Where(g => g.Count() > 1)
                .ToArray();

            // if there are no duplicate registrations then return
            if (!duplicatedImplementationRegistrations.Any()) return;

            const string seperator = @"

";
            // throw the error message about the duplicate registrations
            var inner = string.Join(String.Empty, duplicatedImplementationRegistrations.Select(g => string.Format(Resources.TInjector_RootFactory_DuplicateImplementerRegistrationsInner, g.Key.FullName, string.Join(seperator, g.Select(r => r.CreationStackTrace)))));
            throw new InvalidOperationException(string.Format(Resources.TInjector_RootFactory_DuplicateImplementerRegistrationsOuter, inner));
        }
    }
}