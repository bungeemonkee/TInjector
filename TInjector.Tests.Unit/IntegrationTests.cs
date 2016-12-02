using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TInjector.Locator;
using TInjector.Registration;

namespace TInjector.Tests.Unit
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IntegrationTests
    {
        [TestMethod]
        public void Test_01()
        {
            var registrationCollection = new RegistrationCollection
            {
                new FluentRegistration<Service1>(r => new Service1())
                    .As<Service1, IService1>()
                    .InScope(Scope.Transient),
            };

            var locator = new RootLocator(registrationCollection);

            var service = locator.Get<IService1>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(Service1));
        }

        [TestMethod]
        public void Test_02()
        {
            var registrationCollection = new RegistrationCollection
            {
                new FluentRegistration<Service1>(r => new Service1())
                    .As<Service1, IService1>()
                    .InScope(Scope.Transient),

                new FluentRegistration<Service2>(r => new Service2(r.Get<IService1>()))
                    .As<Service2, IService2>()
                    .InScope(Scope.Transient),
            };

            var locator = new RootLocator(registrationCollection);

            var service = locator.Get<IService2>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(Service2));
        }

        [TestMethod]
        public void Test_03()
        {
            var registrationCollection = new RegistrationCollection
            {
                new FluentRegistration<Service1>(r => new Service1())
                    .As<Service1, IService1>()
                    .InScope(Scope.Singleton),

                new FluentRegistration<Service2>(r => new Service2(r.Get<IService1>()))
                    .As<Service2, IService2>()
                    .InScope(Scope.Graph),

                new FluentRegistration<Service3>(r => new Service3(r.Get<IService1>(), r.Get<IService2>(), r.Get<IService2>()))
                    .As<Service3, IService3>()
                    .InScope(Scope.Transient),
            };

            var locator = new RootLocator(registrationCollection);

            var serviceA = locator.Get<IService3>();
            var serviceB = locator.Get<IService3>();
            
            Assert.AreNotSame(serviceA, serviceB);
            Assert.AreSame(serviceA.Service1, serviceA.Service2A.Value);
            Assert.AreSame(serviceA.Service1, serviceA.Service2B.Value);
            Assert.AreSame(serviceA.Service1, serviceB.Service1);
            Assert.AreSame(serviceA.Service2A, serviceA.Service2B);
            Assert.AreNotSame(serviceA.Service2A, serviceB.Service2A);
        }
    }
}
