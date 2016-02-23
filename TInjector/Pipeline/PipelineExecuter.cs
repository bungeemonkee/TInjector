// TInjector: TInjector
// PipelineExecuter.cs
// Created: 2015-11-01 4:55 PM

using System;
using System.Collections.Generic;
using System.Linq;

namespace TInjector.Pipeline
{
    public class PipelineExecuter
    {
        public readonly IBuilderGenerator BuilderGenerator;
        public readonly IConstructorSelector ConstructorSelector;
        public readonly IDependencyCollector DependencyCollector;
        public readonly IValidator Validator;

        public PipelineExecuter()
        {
            ConstructorSelector = new ConstructorSelector();
            DependencyCollector = new DependencyCollector();
            BuilderGenerator = new BuilderGenerator();
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

            // validate the services
            Validator.Execute(dependencies);

            // create all the builders
            var builders = BuilderGenerator.Execute(dependencies);

            // return the set of builders
            return builders;
        }
    }
}