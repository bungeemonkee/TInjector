using System;
using System.Collections.Generic;

namespace TInjector.Registration
{
    public interface IRegistrationCollection : IEnumerable<IRegistration>
    {
        IRegistration GetRegistration(Type service);
    }
}
