using System;
using System.Linq.Expressions;
using TInjector.Locator;

namespace TInjector.Factory
{
    public class ExpressionFactory<T> : IFactory
        where T : class
    {
        public readonly Func<IRequest, T> Func;

        public ExpressionFactory(Expression<Func<IRequest, T>> expression)
        {
            Func = expression.ReduceAndCompile();
        }

        public object Make(IRequest request)
        {
            return Func(request);
        }
    }
}
