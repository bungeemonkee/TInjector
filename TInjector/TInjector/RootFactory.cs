// TInjector: TInjector
// RootFactory.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-17 8:28 PM

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TInjector.Localization;
using TInjector.Registration;

namespace TInjector
{
    public class RootFactory : List<IRegistrationModule>, IRootFactory
    {
        public IRoot GetRoot()
        {
            // get all the registrations and lock them
            var lockedRegistrations = this.SelectMany(m => m.GenerateRegistrations())
                .Select(r => new LockedRegistration(r))
                .ToArray();

            // make sure no implementer is registered more than once
            ValidateDuplicateImplementerRegistrations(lockedRegistrations);

            // TODO: Check for duplicated service registrations where there is no default selected
            // TODO: Should this even be an error? Or do we just pick the first one?
            // TODO: Should we allow default services for list of services? Should requesting a single instance of a duplicated service be an error?

            // wrap the registrations in a BlockingCollection so we can do the cpu-intensive constructor selection in multiple threads
            // TODO: We may be able to implement an IProducerConsumerCollection that just wraps an enumerable and avoid having to use a provider thread
            var queue = new BlockingCollection<IRegistration>();

            // create twice as many tasks as logical process to make sure we keep them all busy
            // one task will populate the blocking collection
            // the others will read from it and figure out which constructor to use
            var tasks = Enumerable.Range(0, Environment.ProcessorCount*2)
                .Select(i => i == 0
                    ? Task.Run(() => ProvideRegistrations(queue, lockedRegistrations))
                    : Task.Run(() => SelectConstructors(queue)))
                .ToArray();

            // wait for all the registration constructor selection to finish
            Task.WaitAll(tasks);

            // TODO: Give the registrations to the new root object

            // TODO: Return the new root object

            throw new NotImplementedException();
        }

        public void Register(params IRegistrationModule[] modules)
        {
            // Just save the registration, no validation yet - just wait to build the container
            foreach (var module in modules)
            {
                Add(module);
            }
        }

        private void ProvideRegistrations(BlockingCollection<IRegistration> queue, IEnumerable<IRegistration> registrations)
        {
            // for each registration...
            foreach (var registration in registrations)
            {
                // add the registration to the queue
                queue.Add(registration);
            }

            // mark the queue as complete
            queue.CompleteAdding();
        }

        private void SelectConstructors(BlockingCollection<IRegistration> queue)
        {
            IRegistration registration;

            // while there is anything left in the queue...
            while (!queue.IsCompleted && queue.TryTake(out registration))
            {
                // select the constructor to use for the registration
                ConstructorSelector(registration);
            }
        }

        private void ConstructorSelector(IRegistration registration)
        {
            // TODO: Constructor selection algorithm?

            throw new NotImplementedException();
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