using System;

namespace TInjector.Registration
{
    public interface IRegistrationProvider
    {
        IRegistration GetRegistration(Type service);
    }
}
