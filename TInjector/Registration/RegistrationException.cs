using System;

namespace TInjector.Registration
{
    public class RegistrationException : Exception
    {
        public readonly Type Service;

        protected RegistrationException(Type service, Exception inner, string message)
            : base(message, inner)
        {
            Service = service;
        }

        public static RegistrationException GetServiceNotRegisteredException(Type service)
        {
            const string format = @"Multiple registrations found for service '{0}'.";

            return new RegistrationException(service, null, string.Format(format, service));
        }
    }
}
