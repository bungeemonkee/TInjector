// TInjector: TInjector.Examples
// SimpleRegistrationModule.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-18 11:32 AM

using TInjector.Examples.Services;
using TInjector.Registration;

namespace TInjector.Examples.Simple
{
    public class SimpleRegistrationModule : RegistrationModule
    {
        protected override void BuildRegistrations()
        {
            Register<Service>().AsService<IService>();
        }
    }
}