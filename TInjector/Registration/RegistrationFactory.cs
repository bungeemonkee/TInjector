using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TInjector.Registration
{
    public class RegistrationFactory : List<IRegistration<object>>, IRegistrationFactory
    {
        public FluentRegistration<T> Register<T> (IFactory<T> factory)
            where T : class
        {
            var registration = new FluentRegistration<T>(factory);
            Add(registration);
            return registration;
        }

        public FluentRegistration<T> Register<T>(Expression<Func<IRequest, T>> expression)
            where T : class
        {
            var registration = new FluentRegistration<T>(expression);
            Add(registration);
            return registration;
        }
    }
}
