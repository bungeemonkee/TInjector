// TInjector: TInjector
// Root.cs
// Created: 2015-10-17 10:23 AM
// Modified: 2015-10-17 8:28 PM

using System;

namespace TInjector
{
    public class Root : IRoot
    {
        public T Get<T>()
        {
            throw new NotImplementedException();
        }

        public object Get(Type service)
        {
            throw new NotImplementedException();
        }
    }
}