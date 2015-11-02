// TInjector: TInjector
// RootFactory.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-11-01 10:16 PM

using System.Collections.Generic;
using TInjector.Pipeline;

namespace TInjector
{
    public class RootFactory : List<IRegistrationGenerator>, IRootFactory
    {
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