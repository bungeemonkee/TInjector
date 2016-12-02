using System.Linq;
using TInjector.Factory;
using TInjector.Reflection.Factory;
using TInjector.Registration;

namespace TInjector.Reflection.Registration
{
    public class ReflectedRegistration<T> : FluentRegistration<T>
        where T : class
    {
        public ReflectedRegistration()
            : this(new ReflectedFactory<T>())
        { }

        public ReflectedRegistration(IFactory factory)
            : base(factory)
        {
            var services = Implementer
                .FindInterfaces(ServicesToIgnore.TypeFilter, null)
                .ToArray();

            AddServices(services);
        }
    }
}
