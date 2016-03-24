// TInjector: TInjector
// ServiceRegistration.cs
// Created: 2015-11-01 5:26 PM

using System;
using TInjector.Registration;

namespace TInjector.Pipeline
{
    public class ServiceRegistration
    {
        public readonly IRegistration Registration;
        public readonly Type Service;

        public ServiceRegistration(Type service, IRegistration registration)
        {
            Service = service;
            Registration = registration;
        }
    }
}