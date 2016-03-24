using System.Collections.Generic;

namespace TInjector.Registration
{
    public interface IRegistrationFactory : IEnumerable<IRegistration<object>>
    {
    }
}
