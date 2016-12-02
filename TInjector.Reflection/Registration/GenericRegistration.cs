using TInjector.Reflection.Factory;
using TInjector.Registration;

namespace TInjector.Reflection.Registration
{
    public class GenericRegistration<TService, TImplementor> : FluentRegistration<TImplementor>
        where TImplementor : class, TService
    {
        public GenericRegistration()
            : base(new ReflectedFactory<TImplementor>())
        {
            AddServices(typeof(TService));
        }
    }
}
