using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace TInjector
{
    public class ExpressionFactory<T> : IFactory<T>
        where T : class
    {
        public readonly Func<IRequest, T> Func;

        public ExpressionFactory(Expression<Func<IRequest, T>> expression)
        {
            Func = expression.ReduceAndCompile();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Make(IRequest request)
        {
            return Func(request);
        }
    }
}
