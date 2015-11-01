// TInjector: TInjector
// Validator.cs
// Created: 2015-11-01 5:02 PM
// Modified: 2015-11-01 6:31 PM

using System;
using System.Collections.Generic;
using System.Linq;
using TInjector.Localization;

namespace TInjector.Pipeline
{
    public class Validator : IValidator
    {
        public void Execute(ILookup<Type, ServiceRegistrationConstructorDependencies> services)
        {
            // ensure there are no implementers registered multiple times
            ValidateDuplicateImplementerRegistrations(services);

            // TODO: Check for duplicated service registrations where there is no default selected
            // TODO: Should this even be an error?
            // TODO: Or do we only support it as a list?
            // TODO: Should we allow default services for list of services?
            // TODO: Should requesting a single instance of a duplicated service be an error?

            // ensure that every implementer has a constructor selected
            ValidateImplementorsWithNoConstructor(services);

            // TODO: ensure that every implementer has all its dependencies
        }

        private static void ValidateDuplicateImplementerRegistrations(IEnumerable<IGrouping<Type, ServiceRegistrationConstructorDependencies>> services)
        {
            // get all the registrations grouped by implementer
            var duplicatedImplementationRegistrations = services
                .SelectMany(s => s)
                .GroupBy(r => r.Registration.Implementer)
                .Where(g => g.Count() > 1)
                .ToArray();

            // if there are no duplicate registrations then return
            if (!duplicatedImplementationRegistrations.Any()) return;

            const string seperator = @"

";
            // throw the error message about the duplicate registrations
            var inner = string.Join(String.Empty, duplicatedImplementationRegistrations.Select(g => string.Format(Resources.TInjector_Pipeline_Validator_DuplicateImplementerRegistrationsInner, g.Key.FullName, string.Join(seperator, g.Select(r => r.Registration.CreationStackTrace)))));
            throw new InvalidOperationException(string.Format(Resources.TInjector_Pipeline_Validator_DuplicateImplementerRegistrationsOuter, inner));
        }

        private static void ValidateImplementorsWithNoConstructor(IEnumerable<IGrouping<Type, ServiceRegistrationConstructorDependencies>> services)
        {
            // look for registrations that have no viable constructor
            var implementersWithNoConstructor = services
                // flatten the list of registrations
                .SelectMany(s => s)
                // find registrations where we didn't get a constructor
                .Where(r => r.Constructor == null)
                // now get the type name for reporting
                .Select(r => r.Registration.Implementer.FullName)
                // save it
                .ToArray();

            // if there are no registrations with no valid constructor return
            if (implementersWithNoConstructor.Length <= 0) return;

            const string separator = @"
 * ";
            throw new InvalidOperationException(string.Format(Resources.TInjector_Pipeline_Validator_NoValidConstructor, string.Join(separator, implementersWithNoConstructor)));
        }
    }
}