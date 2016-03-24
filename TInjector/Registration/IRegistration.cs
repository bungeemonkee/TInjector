using System;

namespace TInjector.Registration
{
    public interface IRegistration<out T>
        where T : class
    {
        Scope Scope { get; }

        Type Implementer { get; }

        IFactory<T> Factory { get; }

        Type[] Services { get; }

        string CreationStackTrace { get; }
    }
}
