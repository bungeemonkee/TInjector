// TInjector: TInjector
// Validator.cs
// Created: 2015-11-01 5:02 PM

using System;
using System.Collections.Generic;
using System.Linq;

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

            const string format = @"{0} was registered from these locations:
{1}
";
            const string seperator = @"

";

            const string outer = @"Types are registered more than once.
{0}
";

            // throw the error message about the duplicate registrations
            var inner = string.Join(string.Empty, duplicatedImplementationRegistrations.Select(g => string.Format(format, g.Key.FullName, string.Join(seperator, g.Select(r => r.Registration.CreationStackTrace)))));
            throw new InvalidOperationException(string.Format(outer, inner));
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

            const string format = @"Unable to create builders for the following types as they have no public constructor where all parameters can also be resolved:
 * {0}";
            const string separator = @"
 * ";
            throw new InvalidOperationException(string.Format(format, string.Join(separator, implementersWithNoConstructor)));
        }
    }
}