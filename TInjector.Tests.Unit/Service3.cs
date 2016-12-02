
using System.Diagnostics.CodeAnalysis;

namespace TInjector.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class Service3 : IService3
    {
        public IService1 Service1 { get; }

        public IService2 Service2A { get; }

        public IService2 Service2B { get; }

        public Service3(IService1 service1, IService2 service2A, IService2 service2B)
        {
            Service1 = service1;
            Service2A = service2A;
            Service2B = service2B;
        }
    }
}
