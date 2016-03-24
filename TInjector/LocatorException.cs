using System;

namespace TInjector
{
    public class RequestException : Exception
    {
        public readonly Type Service;
        public readonly IRequest Request;

        protected RequestException(Type service, IRequest request, string message)
            : base(message)
        {
            Service = service;
            Request = request;
        }

        protected RequestException(Type service, IRequest request, Exception inner, string message)
            : base(message, inner)
        {
            Service = service;
            Request = request;
        }

        public static RequestException GetServiceNotRegisteredException(Type service)
        {
            const string format = @"Unable to resolve request for service '{0}' as no such service is registered.";

            return new RequestException(service, null, string.Format(format, service));
        }

        public static RequestException GetInvalidScopeException(Type service, Scope scope)
        {
            const string format = @"Unable to resolve request for service '{0}' as it is registered with an invalid scope: '{1}'.";

            return new RequestException(service, null, string.Format(format, service, scope));
        }

        public static RequestException GetFactoryException(IRequest request, Exception inner)
        {
            const string format = @"Unable to resolve request for service '{0}' due to an exception. See the InnerException for more details.";

            return new RequestException(request.Service, request, inner, string.Format(format, request.Service));
        }
    }
}
