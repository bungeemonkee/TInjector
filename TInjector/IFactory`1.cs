
namespace TInjector
{
    public interface IFactory<T> : IFactory
    {
        T Make(IRequest<T> request);
    }
}
