using System.Diagnostics.CodeAnalysis;

namespace TInjector.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class Service2 : IService2
    {
        public IService1 Value { get; }

        public Service2(IService1 service1)
        {
            Value = service1;
        }
    }
}
