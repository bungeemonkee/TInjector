using System;

namespace TInjector
{
    public interface IRequest : ILocator
    {
        IRequest Parent { get; }

        Type Service { get; }

        Type Implementer { get; }

        Scope Scope { get; }
    }
}
