using System;
using System.Collections.Generic;

namespace TInjector.Initialization
{
    public class RegistrationModule : List<IRegistration>, IRegistrationModule
    {
        protected IRegistration Register<T>()
        {
            // TODO: Save the stack trace where the registration was created

            // TODO: Add the registration to the list, no validation yet wait for building the container

            throw new NotImplementedException();
        }
    }
}
