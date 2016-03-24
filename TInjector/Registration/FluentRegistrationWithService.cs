using System;

namespace TInjector.Registration
{
    public class FluentRegistrationWithService<T, TService> : FluentRegistration<T>
        where T : class, TService
    {
        public Type ServiceToRegister { get; protected set; }

        public FluentRegistrationWithService(FluentRegistration<T> parent)
            : base(parent)
        {
            ServiceToRegister = typeof(TService);

            AddServices(ServiceToRegister);
        }
    }
}
