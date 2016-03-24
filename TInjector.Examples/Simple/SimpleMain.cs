using TInjector.Examples.Services;
using TInjector.Registration;

namespace TInjector.Examples.Simple
{
    public static class SimpleMain
    {
        public static void SimpleMain_01()
        {
            var registrationFactory = new RegistrationFactory
            {
                new FluentRegistration<Service>(r => new Service())
                    .As<Service, IService>()
                    .InScope(Scope.Transient)
            };

            var locatorFactory = new LocatorFactory
            {
                registrationFactory
            };

            var locator = locatorFactory.GetLocator();

            var service = locator.Get<IService>();

            service.DoAThing();
        }
    }
}
