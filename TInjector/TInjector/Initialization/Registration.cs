using System;

namespace TInjector.Initialization
{
    public class Registration<T> : IRegistration
    {
        public Type Type { get; private set; }

        public Registration()
        {
            Type = typeof(T);
        }
    }
}
