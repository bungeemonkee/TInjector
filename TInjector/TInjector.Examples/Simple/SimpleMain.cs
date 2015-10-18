// TInjector: TInjector.Examples
// SimpleMain.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-18 11:32 AM

using TInjector.Examples.Services;

namespace TInjector.Examples.Simple
{
    public static class SimpleMain
    {
        public static void Main()
        {
            var factory = new RootFactory();
            factory.Register(new SimpleRegistrationModule());
            var root = factory.GetRoot();
            var service = root.Get<IService>();
            service.DoAThing();
        }
    }
}