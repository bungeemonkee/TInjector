using System;
using System.Linq;
using System.Reflection;
using TInjector.Factory;
using TInjector.Locator;

namespace TInjector.Reflection.Factory
{
    public class ReflectedFactory<T> : IFactory
        where T : class
    {
        private ReflectedConstructor<T> _constructor;

        public object Make(IRequest request)
        {
            if (_constructor != null) return _constructor.Invoke(request);

            lock (this)
            {
                if (_constructor == null)
                {
                    _constructor = GetConstructorInvoker(request);
                }
            }

            return _constructor.Invoke(request);
        }

        protected virtual ConstructorInfo SelectConstructor(Func<Type, bool> isRegistered)
        {
            // Get the public constructor where all parameters are registered that has the most parameters
            return typeof(T)
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => new
                {
                    Constructor = x,
                    Parameters = x.GetParameters()
                })
                .Select(x => new
                {
                    x.Constructor,
                    x.Parameters,
                    RegisteredParametersCount = x.Parameters.Count(y => isRegistered(y.ParameterType))
                })
                .Where(x => x.Parameters.Length == x.RegisteredParametersCount)
                .OrderByDescending(x => x.RegisteredParametersCount)
                .Select(x => x.Constructor)
                .First();
        }

        private ReflectedConstructor<T> GetConstructorInvoker(IRequest request)
        {
            var constructor = SelectConstructor(x => request.Registrations.GetRegistration(x) != null);
            var parameterTypes = constructor.GetParameters().Select(x => x.ParameterType).ToArray();
            
            return new ReflectedConstructor<T>(constructor, parameterTypes);
        }
    }
}
