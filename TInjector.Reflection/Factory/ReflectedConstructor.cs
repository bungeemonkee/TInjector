using System;
using System.Linq;
using System.Reflection;
using TInjector.Locator;

namespace TInjector.Reflection.Factory
{
    public class ReflectedConstructor<T>
    {
        public ConstructorInfo Constructor { get; }

        public Type[] ParameterTypes { get; }

        public ReflectedConstructor(ConstructorInfo constructor, Type[] parameterTypes)
        {
            Constructor = constructor;
            ParameterTypes = parameterTypes;
        }

        public T Invoke(IRequest request)
        {
            var arguments = ParameterTypes.Select(request.Get).ToArray();
            return (T)Constructor.Invoke(arguments);
        }
    }
}
