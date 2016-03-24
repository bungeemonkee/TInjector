
namespace TInjector.Registration
{
    public static class FluentRegistrationExtensions
    {
        public static FluentRegistrationWithService<T, TService> As<T, TService>(this FluentRegistration<T> registration)
            where T : class, TService
        {
            return new FluentRegistrationWithService<T, TService>(registration);
        }
    }
}
