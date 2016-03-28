
namespace TInjector
{
    public interface IFactory
    {
        object Make(IRequest request);
    }
}
