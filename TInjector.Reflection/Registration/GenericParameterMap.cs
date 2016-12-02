using System;
using System.Linq;

namespace TInjector.Reflection.Registration
{
    public class GenericParameterMap
    {
        public Type Implementor { get; }

        public int[] ParameterOrder { get; }

        public static GenericParameterMap Create(Type service, Type implementor)
        {
            var serviceParametersByName = service.GetGenericArguments().Select(x => x.Name).ToArray();
            var implementorParametersByName = implementor.GetGenericArguments().Select(x => x.Name).ToArray();

            if (serviceParametersByName.Length != implementorParametersByName.Length) return null;

            var parameterOrder = new int[serviceParametersByName.Length];

            for (var i = 0; i < serviceParametersByName.Length; ++i)
            {
                var name = serviceParametersByName[i];
                var implementorIndex = Array.IndexOf(implementorParametersByName, name);
                if (implementorIndex < 0) return null;

                parameterOrder[i] = implementorIndex;
            }

            return new GenericParameterMap(implementor, parameterOrder);
        }

        private GenericParameterMap(Type implementor, int[] parameterOrder)
        {
            Implementor = implementor;
            ParameterOrder = parameterOrder;
        }

        public Type[] RearrangeParameters(Type[] serviceParameters)
        {
            var result = new Type[ParameterOrder.Length];

            for (var i = 0; i < ParameterOrder.Length; ++i)
            {
                var resultIndex = ParameterOrder[i];
                result[resultIndex] = serviceParameters[i];
            }

            return result;
        }
    }
}
