using System;

namespace TInjector.Registration
{
    public interface IRegistration<T> : IRegistration
    {
        new IFactory<T> Factory { get; }

        new Action<IRequest, T>[] ActivationCallbacks { get; }
    }
}
