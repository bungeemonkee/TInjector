using System;

namespace TInjector.Locator
{
    public class LocatorException : Exception
    {
        public readonly Type Service;
        public readonly IRequest Request;

        protected LocatorException(Type service, IRequest request, string message)
            : base(message)
        {
            Service = service;
            Request = request;
        }

        protected LocatorException(Type service, IRequest request, Exception inner, string message)
            : base(message, inner)
        {
            Service = service;
            Request = request;
        }

        public static LocatorException GetServiceNotRegisteredException(Type service)
        {
            const string format = @"Unable to resolve request for service '{0}' as no such service is registered.";

            return new LocatorException(service, null, string.Format(format, service));
        }

        public static LocatorException GetInvalidScopeException(Type service, Scope scope)
        {
            const string format = @"Unable to resolve request for service '{0}' as it is registered with an invalid scope: '{1}'.";

            return new LocatorException(service, null, string.Format(format, service, scope));
        }

        public static LocatorException GetFactoryException(IRequest request, Exception inner)
        {
            const string format = @"Unable to resolve request for service '{0}' due to an exception when invoking the factory. See the InnerException for more details.";

            return new LocatorException(request.Service, request, inner, string.Format(format, request.Service));
        }

        public static LocatorException GetActivationCallbackException(IRequest request, Exception inner)
        {
            const string format = @"Unable to resolve request for service '{0}' due to an exception when invoking an activation callback. See the InnerException for more details.";

            return new LocatorException(request.Service, request, inner, string.Format(format, request.Service));
        }
    }
}
