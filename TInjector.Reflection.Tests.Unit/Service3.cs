using System.Diagnostics.CodeAnalysis;

namespace TInjector.Reflection.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class Service3<T1, T2> : IService3<T2, T1>
    {
        public T1 Value1 { get; }

        public T2 Value2 { get; }
    }
}
