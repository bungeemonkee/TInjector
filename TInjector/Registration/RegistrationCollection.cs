using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TInjector.Registration
{
    public class RegistrationCollection : IRegistrationProvider, IEnumerable<IRegistration>
    {
        private readonly IDictionary<Type, IRegistration> _registrations;

        public RegistrationCollection()
        {
            _registrations = new Dictionary<Type, IRegistration>();
        }

        public RegistrationCollection(params RegistrationCollection[] registrationCollections)
        {
            _registrations = registrationCollections
                .SelectMany(x => x)
                .SelectMany(x => x.Services, (x, y) => new
                {
                    Service = y,
                    Registration = x
                })
                .ToDictionary(x => x.Service, x => x.Registration);
        }

        public IEnumerator<IRegistration> GetEnumerator()
        {
            return _registrations.Values.GetEnumerator();
        }

        public IRegistration GetRegistration(Type service)
        {
            IRegistration result;
            _registrations.TryGetValue(service, out result);

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IRegistration registration)
        {
            foreach (var service in registration.Services)
            {
                _registrations.Add(service, registration);
            }
        }
    }
}
