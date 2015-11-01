// TInjector: TInjector
// PipelineExecuter.cs
// Created: 2015-11-01 4:55 PM
// Modified: 2015-11-01 6:31 PM

using System;
using System.Collections.Generic;
using System.Linq;
using TInjector.Build;

namespace TInjector.Pipeline
{
    public class PipelineExecuter
    {
        public readonly IConstructorSelector ConstructorSelector;
        public readonly IDependencyCollector DependencyCollector;
        public readonly IValidator Validator;

        public PipelineExecuter()
        {
            ConstructorSelector = new ConstructorSelector();
            DependencyCollector = new DependencyCollector();
            Validator = new Validator();
        }

        public IDictionary<Type, IBuilder> Process(IEnumerable<IRegistrationGenerator> registrationGenerations)
        {
            // TODO: Attempt to parallelize as much of this as possible

            // get all the registrations by their services
            var services = registrationGenerations
                .SelectMany(r => r.Execute())
                .SelectMany(r => r.Services, (r, s) => new ServiceRegistration(s, r))
                .ToLookup(r => r.Service, r => r);

            // select constructors for each service
            var constructors = ConstructorSelector.Execute(services);

            // collect all the dependencies for each service
            var dependencies = DependencyCollector.Execute(constructors);

            // create the storage for the builders
            var buildersByService = new Dictionary<Type, IBuilder>(dependencies.Count);

            foreach (var registration in dependencies)
            {
                if (buildersByService.ContainsKey(registration.Key)) continue;

                // TODO: Need to re-work builder creation
            }

            /*
            // now create builders for every service
            var buildersByService = registrationsByServiceWithConstructors
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
            */

            // validate the services
            Validator.Execute(dependencies);

            // TODO: Actually create builders
            return null;
        }
    }
}