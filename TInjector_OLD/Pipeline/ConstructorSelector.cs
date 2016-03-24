// TInjector: TInjector
// ConstructorSelector.cs
// Created: 2015-11-01 5:38 PM

using System;
using System.Linq;
using System.Reflection;

namespace TInjector.Pipeline
{
    public class ConstructorSelector : IConstructorSelector
    {
        public ILookup<Type, ServiceRegistrationConstructor> Execute(ILookup<Type, ServiceRegistration> services)
        {
            // TODO: Parallelize constructor selection

            // select a constructor for each registration
            return services
                // get all the registrations
                .SelectMany(rbs => rbs)
                // select a constructor for each registration
                .Select(r => SelectConstructor(services, r))
                // convert to the appropriate lookup
                .ToLookup(r => r.Service);
        }

        private static ServiceRegistrationConstructor SelectConstructor(ILookup<Type, ServiceRegistration> services, ServiceRegistration service)
        {
            // get all the implementer's public constructors...
            var constructorWithParameters = service.Registration.Implementer.GetConstructors(BindingFlags.Public)
                // get the parameters for the constructors
                .Select(c => new
                {
                    Constructor = c,
                    Parameters = c.GetParameters()
                })
                // make sure we can resolve all the parameters for the constructor
                .Where(cwp => cwp.Parameters.All(p => services.Contains(p.ParameterType)))
                // order by the number of parameters - the constructor with the most parameters we can build will be first
                .OrderByDescending(cwp => cwp.Parameters.Length)
                // pick the first one
                .FirstOrDefault();

            // get the constructor
            var constructor = constructorWithParameters != null
                ? constructorWithParameters.Constructor
                : null;

            // get the parameters as an array of types
            var parameters = constructorWithParameters != null
                ? constructorWithParameters.Parameters != null && constructorWithParameters.Parameters.Length > 0
                    ? constructorWithParameters.Parameters.Select(p => p.ParameterType).ToArray()
                    : null
                : null;

            return new ServiceRegistrationConstructor(service, constructor, parameters);
        }
    }
}