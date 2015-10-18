// TInjector: TInjector
// AutomaticRegistrationModule.cs
// Created: 2015-10-17 7:02 PM
// Modified: 2015-10-17 8:28 PM

namespace TInjector.Registration
{
    /// <summary>
    ///     A registration module that registers every public class from the assembly for the given type to (almost) every
    ///     interface it implements.
    ///     Note: Some system interfaces are excluded from registration. See the documentation for
    ///     TInjector.Initialization.UnlockedRegistration.AddAllServices.
    /// </summary>
    /// <typeparam name="T">A type in the assembly containing all the types to register.</typeparam>
    public class AutomaticRegistrationModule<T> : RegistrationModule
    {
        private readonly string _inNamespace;

        public AutomaticRegistrationModule()
            : this(null)
        {
        }

        public AutomaticRegistrationModule(string inNamespace)
        {
            _inNamespace = inNamespace;
        }

        protected override void BuildRegistrations()
        {
            RegisterAll<T>(_inNamespace);
        }
    }
}