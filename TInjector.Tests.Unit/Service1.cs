
using System.Diagnostics.CodeAnalysis;

namespace TInjector.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class Service1 : IService1
    {
        public string Value { get; }

        public Service1()
        {
            Value = "kfnnppa";
        }
    }
}
