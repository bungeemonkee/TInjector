using System;

namespace TInjector.Initialization
{
    /// <summary>
    /// All functionality required for type registrations
    /// </summary>
    public interface IRegistration
    {
        Type Type { get; }
    }
}
