// TInjector: TInjector
// RootFactory.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-11-01 6:31 PM

using System;
using System.Collections.Generic;
using TInjector.Pipeline;

namespace TInjector
{
    public class RootFactory : List<IRegistrationGenerator>, IRootFactory
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
            // TODO: This while function is essentially a pipeline that caches at a few steps to speed lookups, we may want to formalize that pipeline
            var pipeline = new PipelineExecuter();
            var builders = pipeline.Process(this);

            // create and return the new root
            return new Root(builders);
        }

        public void Register(params IRegistrationGenerator[] modules)
        {
            // Just save the registration, no validation yet - just wait to build the container
            foreach (var module in modules)
            {
                Add(module);
            }
        }
    }
}