using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TInjector.Registration
{
    public class RegistrationCollection : IRegistrationCollection
    {
        private readonly IDictionary<Type, IRegistration> _registrations;

        public RegistrationCollection()
        {
            _registrations = new Dictionary<Type, IRegistration>();
        }

        public RegistrationCollection(params IRegistrationCollection[] registrationCollections)
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
            return _registrations[service];
        }

        public IRegistration<T> GetRegistration<T>()
        {
            return (IRegistration<T>)_registrations[typeof(T)];
        }

        public void Add<T>(IRegistration<T> registration)
        {
            foreach (var service in registration.Services)
            {
                _registrations[service] = registration;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IRegistration registration)
        {
            foreach (var service in registration.Services)
            {
                _registrations[service] = registration;
            }
        }
    }
}
