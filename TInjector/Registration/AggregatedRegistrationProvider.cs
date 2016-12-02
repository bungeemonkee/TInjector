using System;
using System.Collections.Generic;
using System.Linq;

namespace TInjector.Registration
{
    public class AggregatedRegistrationProvider : IRegistrationProvider
    {
        private readonly IRegistrationProvider[] _providers;
        private readonly IDictionary<Type, IRegistration> _registrationCache = new Dictionary<Type, IRegistration>();

        public AggregatedRegistrationProvider(params IRegistrationProvider[] providers)
        {
            _providers = providers;
        }

        public IRegistration GetRegistration(Type service)
        {
            IRegistration result;
            if (_registrationCache.TryGetValue(service, out result)) return result;

            lock (service)
            {
                if (_registrationCache.TryGetValue(service, out result)) return result;

                var all = _providers.Select(x => x.GetRegistration(service)).Where(x => x != null).ToList();

                switch (all.Count)
                {
                    case 0:
                        break;
                    case 1:
                        result = all[0];
                        break;
                    default:
                        throw RegistrationException.GetServiceNotRegisteredException(service);
                }

                _registrationCache.Add(service, result);
            }

            return result;
        }
    }
}
