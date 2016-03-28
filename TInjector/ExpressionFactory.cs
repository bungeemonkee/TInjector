using System;
using System.Linq.Expressions;

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

        public T Make(IRequest<T> request)
        {
            return (T)((IFactory)this).Make(request);
        }

        object IFactory.Make(IRequest request)
        {
            return Func(request);
        }
    }
}
