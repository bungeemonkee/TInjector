using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TInjector.Reflection.Registration
{
    public static class ServicesToIgnore
    {
        public static readonly Type[] All =
        {
            typeof(IDisposable),
            typeof(IComponent)
        };

        public static readonly TypeFilter TypeFilter = (t, o) => !All.Contains(t);
    }
}
