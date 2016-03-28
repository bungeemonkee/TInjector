using TInjector.Examples.Services;
using TInjector.Locator;
using TInjector.Registration;

namespace TInjector.Examples.Simple
{
    public static class SimpleMain
    {
        public static void SimpleMain_01()
        {
            var registrationCollection = new RegistrationCollection
            {
                new FluentRegistration<Service>(r => new Service())
                    .As<Service, IService>()
                    .InScope(Scope.Transient)
            };

            var locator = new RootLocator(registrationCollection);

            var service = locator.Get<IService>();

            service.DoAThing();
        }
    }
}
