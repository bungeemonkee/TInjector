using TInjector.Locator;
using TInjector.Registration;

namespace TInjector.Request
{
    public class SimpleRequest<T> : SimpleRequest, IRequest<T>
    {
        public SimpleRequest(ILocator locator, IRegistrationCollection registrations, IRequest parent)
            : base(locator, registrations, parent, typeof(T))
        { }
    }
}
