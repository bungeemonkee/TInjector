using System;
using System.Linq;
using System.Reflection;

namespace TInjector.Reflection.Registration
{
    public class ReflectedFactory<T> : IFactory<T>
        where T : class
    {
        private Func<IRequest, T> _constructorInvoker;

        public object Make(IRequest request)
        {
            if (_constructorInvoker == null)
            {
                _constructorInvoker = GetConstructorInvoker(request);
            }

            return _constructorInvoker(request);
        }

        public T Make(IRequest<T> request)
        {
            if (_constructorInvoker == null)
            {
                _constructorInvoker = GetConstructorInvoker(request);
            }

            return _constructorInvoker(request);
        }

        protected virtual ConstructorInfo SelectConstructor(Func<Type, bool> isRegistered)
        {
            // Get the public constructor where all parameters are registered that has the most parameters
            return typeof(T)
                .GetConstructors(BindingFlags.Public)
                .Select(x => new
                {
                    Constructor = x,
                    Parameters = x.GetParameters()
                })
                .Select(x => new
                {
                    Constructor = x.Constructor,
                    Parameters = x.Parameters,
                    RegisteredParametersCount = x.Parameters.Count(y => isRegistered(y.ParameterType))
                })
                .Where(x => x.Parameters.Length == x.RegisteredParametersCount)
                .OrderByDescending(x => x.RegisteredParametersCount)
                .Select(x => x.Constructor)
                .First();
        }

        private Func<IRequest, T> GetConstructorInvoker(IRequest request)
        {
            if (_constructorInvoker != null) return _constructorInvoker;

            var constructor = SelectConstructor(x => request.Registrations.GetRegistration(x) != null);
            var parameters = constructor.GetParameters().Select(x => x.ParameterType).ToArray();

            return (IRequest r) =>
            {
                var arguments = parameters.Select(x => r.Locator.Get(x)).ToArray();
                return (T)constructor.Invoke(arguments);
            };
        }
    }
}
