using System.Diagnostics.CodeAnalysis;

namespace TInjector.Reflection.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class Service2<T> : IService2<T>
    {
        public T Value { get; }
    }
}
