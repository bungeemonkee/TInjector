// TInjector: TInjector
// BuilderGenerator.cs
// Created: 2015-11-01 9:23 PM
// Modified: 2015-11-01 10:16 PM

using System;
using System.Collections.Generic;
using System.Linq;

namespace TInjector.Pipeline
{
    public class BuilderGenerator : IBuilderGenerator
    {
        // the collection types we can safely map to our array builders
        private static readonly Type[] EnumerableTypes =
        {
            typeof (IEnumerable<>),
            typeof (ICollection<>),
            typeof (IList<>)
        };

        public IDictionary<Type, IBuilder> Execute(ILookup<Type, ServiceRegistrationConstructorDependencies> services)
        {
            // create the builder store
            var builders = new Dictionary<Type, IBuilder>(services.Count);

            // create a builder for each service
            foreach (var builder in services.SelectMany(GenerateBuilders))
            {
                builders.Add(builder.Key, builder.Value);
            }

            // return the builders
            return builders;
        }

        private static IEnumerable<KeyValuePair<Type, IBuilder>> GenerateBuilders(IGrouping<Type, ServiceRegistrationConstructorDependencies> service)
        {
            // if there is only one implementer...
            if (!service.Skip(1).Any())
            {
                // create the builder for the given type
                return new Dictionary<Type, IBuilder>(1)
                {
                    {service.Key, new Builder(service.Key, service)}
                };
            }

            // create the array type
            var arrayType = service.Key.MakeArrayType();

            // create the builder
            var builder = new Builder(arrayType, service);

            // create the mappings for all the collections types
            var results = EnumerableTypes.ToDictionary(t => t.MakeGenericType(service.Key), t => (IBuilder) builder);

            // add the mapping for the array type
            results[arrayType] = builder;

            // return the results
            return results;
        }
    }
}