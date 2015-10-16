using System;

namespace TInjector.Initialization
{
    public static class RegistrationUtility
    {
        private static readonly Type RegistrationType = typeof (Registration<>);

        /// <summary>
        /// Make a registration from a type rather than a type parameter.
        /// Note: This uses reflection and should be avoided when possible.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object MakeRegistration(Type t)
        {
            return Activator.CreateInstance(RegistrationType.MakeGenericType(t));
        }
    }
}
