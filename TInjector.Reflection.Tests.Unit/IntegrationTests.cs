using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TInjector.Locator;
using TInjector.Reflection.Registration;

namespace TInjector.Reflection.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IntegrationTests
    {
        [TestMethod]
        public void Test_01()
        {
            var registrationCollection = ReflectedRegistrationCollection.ForAssemblyOfType<IntegrationTests>();

            var locator = new RootLocator(registrationCollection);

            var service = locator.Get<IService1>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(Service1));
        }

        [TestMethod]
        public void Test_02()
        {
            var registrationCollection = ReflectedRegistrationCollection.ForAssemblyOfType<IntegrationTests>();

            var locator = new RootLocator(registrationCollection);

            var service = locator.Get<IService2<string>>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(Service2<string>));
        }

        [TestMethod]
        public void Test_03()
        {
            var registrationCollection = ReflectedRegistrationCollection.ForAssemblyOfType<IntegrationTests>();

            var locator = new RootLocator(registrationCollection);

            var service = locator.Get<IService3<string, int>>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(Service3<int, string>));
        }
    }
}
