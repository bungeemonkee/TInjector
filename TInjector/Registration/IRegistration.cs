using System;
using TInjector.Factory;
using TInjector.Locator;

namespace TInjector.Registration
{
    public interface IRegistration
    {
        Scope Scope { get; }

        Type Implementer { get; }

        IFactory Factory { get; }

        Type[] Services { get; }

        Action<IRequest, object>[] ActivationCallbacks { get; }

        string CreationStackTrace { get; }
    }
}
