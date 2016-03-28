using System;
using System.Collections.Generic;

namespace TInjector.Registration
{
    public interface IRegistrationCollection : IEnumerable<IRegistration>
    {
        IRegistration<T> GetRegistration<T>();

        IRegistration GetRegistration(Type service);
    }
}
