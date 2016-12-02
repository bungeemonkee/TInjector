using System;
using System.Linq.Expressions;
using TInjector.Factory;
using TInjector.Locator;

namespace TInjector.Registration
{
    public class FluentRegistration<T> : IRegistration
        where T : class
    {
        private Type[] _services;
        private Action<IRequest, object>[] _activationCallbacks;

        public string CreationStackTrace { get; }

        public IFactory Factory { get; }

        public Type Implementer { get; }

        public Scope Scope { get; private set; }

        public Type[] Services => _services;

        Action<IRequest, object>[] IRegistration.ActivationCallbacks => _activationCallbacks;

        public FluentRegistration(Expression<Func<IRequest, T>> expression)
            : this(new ExpressionFactory<T>(expression))
        { }

        public FluentRegistration(IFactory factory)
        {
            _services = new Type[0];
            _activationCallbacks = new Action<IRequest, object>[0];

            Factory = factory;
            Implementer = typeof(T);
            Scope = Scope.Transient;

            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                // HACK: This is very non-ideal, but portable libraries don't (currently) support direct stack traces
                CreationStackTrace = ex.StackTrace;
            }
        }

        protected FluentRegistration(FluentRegistration<T> copy)
        {
            _services = copy._services;
            _activationCallbacks = copy._activationCallbacks;

            Factory = copy.Factory;
            Implementer = copy.Implementer;
            Scope = copy.Scope;
            CreationStackTrace = copy.CreationStackTrace;
        }

        public FluentRegistration<T> InScope(Scope scope)
        {
            Scope = scope;
            return this;
        }

        public FluentRegistration<T> AsSelf()
        {
            AddServices(Implementer);
            return this;
        }

        public void WithActivation(Action<IRequest, T> callback)
        {
            var callbacks = new[] { (Action<IRequest, object>)callback };

            // append the items to the existing array
            AddArray(ref _activationCallbacks, callbacks);
        }

        protected void AddServices(params Type[] services)
        {
            // append the items to the existing array
            AddArray(ref _services, services);
        }

        private void AddArray<TArray>(ref TArray[] existingItems, TArray[] newItems)
        {
            if (existingItems == null)
            {
                throw new ArgumentNullException(nameof(existingItems));
            }

            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            if (newItems.Length == 0) return;

            // Resize the new array to be able to hold both
            Array.Resize(ref newItems, newItems.Length + existingItems.Length);

            // Add the current array to the new array
            Array.Copy(_services, 0, newItems, newItems.Length - existingItems.Length, existingItems.Length);

            // Save the new services array
            existingItems = newItems;
        }
    }
}
