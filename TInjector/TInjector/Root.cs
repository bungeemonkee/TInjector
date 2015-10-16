using System;

namespace TInjector
{
    public class Root : IRoot
    {
        public T Get<T>()
        {
            throw new NotImplementedException();
        }
    }
}
