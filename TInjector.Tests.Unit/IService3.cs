namespace TInjector.Tests.Unit
{
    public interface IService3
    {
        IService1 Service1 { get; }
        IService2 Service2A { get; }
        IService2 Service2B { get; }
    }
}