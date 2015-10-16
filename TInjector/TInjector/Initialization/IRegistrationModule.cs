using System.Collections.Generic;

namespace TInjector.Initialization
{
    /// <summary>
    /// Functionality required by any module that produces registrations.
    /// </summary>
    public interface IRegistrationModule : IEnumerable<IRegistration>
    {
    }
}
