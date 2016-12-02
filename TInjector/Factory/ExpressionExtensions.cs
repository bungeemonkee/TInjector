using System.Linq.Expressions;

namespace TInjector.Factory
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Reduces an expression as much as possible and then compiles it into a function.
        /// </summary>
        /// <typeparam name="T">The type of the expression's delegate.</typeparam>
        /// <param name="expression">The expression to reduce and compile.</param>
        /// <returns>The reduced, compiled version of the expression.</returns>
        public static T ReduceAndCompile<T>(this Expression<T> expression)
        {
            var reduced = expression;

            // So long as the expression can be reduced...
            while (reduced != null && reduced.CanReduce)
            {
                // Reduce to an equivalent expression
                reduced = expression.Reduce() as Expression<T>;
            }

            // Compile into the resulting function
            return (reduced ?? expression).Compile();
        }
    }
}
