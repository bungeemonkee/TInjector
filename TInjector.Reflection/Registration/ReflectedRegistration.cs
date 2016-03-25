using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TInjector.Registration;

namespace TInjector.Reflection.Registration
{
    public class ReflectedRegistration<T> : FluentRegistration<T>
        where T : class
    {
        protected static readonly Type[] ServicesToIgnore =
        {
            typeof(IDisposable),
            typeof(IComponent)
        };

        protected static readonly TypeFilter TypeFilter = (t, o) => !ServicesToIgnore.Contains(t);

        public ReflectedRegistration()
            : this(new ReflectedFactory<T>())
        { }

        public ReflectedRegistration(Expression<Func<IRequest, T>> expression)
            : this(new ExpressionFactory<T>(expression))
        { }

        public ReflectedRegistration(IFactory<T> factory)
            : base(factory)
        {
            var services = Implementer
                .FindInterfaces(TypeFilter, null)
                .Where(x => !ServicesToIgnore.Contains(x))
                .ToArray();

            AddServices(services);
        }
    }
}
