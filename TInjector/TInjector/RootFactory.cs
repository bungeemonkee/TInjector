using System;
using System.Collections.Generic;
using System.Linq;
using TInjector.Initialization;

namespace TInjector
{
    public class RootFactory : List<IRegistrationModule>, IRootFactory
    {
        public void Register(params IRegistrationModule[] modules)
        {
            // TODO: Just save the registration, no validation yet - just wait to build the container

            throw new NotImplementedException();
        }

        public IRoot GetRoot()
        {
            // get all the registrations
            var registrations = this.SelectMany(m => m).GroupBy(r => r.Type).ToArray();

            // make sure each concrete type is only registered once
            // TODO: Can be more efficient here - don't need to count everything, we can stop at 2
            var duplicatedImplementationRegistrations = registrations.Where(g => g.Count() > 1).ToArray();

            // if there are dupli8cate registrations
            if (duplicatedImplementationRegistrations.Any())
            {
                // TODO: Throw a (VERY INFORMATIVE) exception about duplicate registrations
            }

            // TODO: Check for duplicated service registrations where there is no default selected
            // TODO: Should this even be an error? Should we handle lists?
            // TODO: SHould we allow default services for list of services? Should requesting a single instance of a duplicated service be an error?

            // TODO: Give the registrations to the new root object

            // TODO: Return the new root object

            throw new NotImplementedException();
        }
    }
}
