using System;

namespace TInjector.Registration
{
    public class FluentRegistrationWithService<T, TService> : FluentRegistration<T>
        where T : class, TService
    {
        public Type Service { get; protected set; }

        public FluentRegistrationWithService(FluentRegistration<T> parent)
            : base(parent)
        {
            Service = typeof(TService);

            AddServices(Service);
        }
    }
}
