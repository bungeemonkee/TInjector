
using TInjector.Locator;

namespace TInjector.Factory
{
    public interface IFactory
    {
        object Make(IRequest request);
    }
}
