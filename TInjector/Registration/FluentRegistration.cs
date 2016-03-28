using System;
using System.Linq.Expressions;

namespace TInjector.Registration
{
    public class FluentRegistration<T> : IRegistration<T>
        where T : class
    {
        private readonly FluentRegistration<T> _parent;
        private string _creationStackTrace;
        private IFactory<T> _factory;
        private Type _implementer;
        private Scope _scope;
        private Type[] _services;
        private Action<IRequest, object>[] _activationCallbacks;

        public string CreationStackTrace => _parent?.CreationStackTrace ?? _creationStackTrace;

        public IFactory<T> Factory => _parent?.Factory ?? _factory;

        public Type Implementer => _parent?.Implementer ?? _implementer;

        public Scope Scope => _parent?.Scope ?? _scope;

        public Type[] Services => _parent?.Services ?? _services;

        public Action<IRequest, T>[] ActivationCallbacks => _parent?.ActivationCallbacks ?? _activationCallbacks;

        IFactory IRegistration.Factory => _parent?.Factory ?? _factory;

        Action<IRequest, object>[] IRegistration.ActivationCallbacks => ((IRegistration)_parent)?.ActivationCallbacks ?? _activationCallbacks;

        public FluentRegistration(Expression<Func<IRequest, T>> expression)
            : this(new ExpressionFactory<T>(expression))
        { }

        public FluentRegistration(IFactory<T> factory)
        {
            _factory = factory;
            _implementer = typeof(T);
            _scope = Scope.Transient;
            _services = new[] { _implementer };

            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                // HACK: This is very non-ideal, but portable libraries don't (currently) support direct stack traces
                _creationStackTrace = ex.StackTrace;
            }
        }

        protected FluentRegistration(FluentRegistration<T> parent)
        {
            _parent = parent;
        }

        public FluentRegistration<T> InScope(Scope scope)
        {
            _scope = scope;
            return this;
        }

        protected void AddServices(params Type[] services)
        {
            if (_parent != null)
            {
                // Add the services to the parent instead
                _parent.AddServices(services);
                return;
            }

            // append the items to the existing array
            AddArray(ref _services, services);
        }

        protected void AddActivationCallbacks(params Action<IRequest, T>[] callbacks)
        {
            if (_parent != null)
            {
                // Add the callbacks to the parent instead
                _parent.AddActivationCallbacks(callbacks);
                return;
            }

            // append the items to the existing array
            AddArray(ref _activationCallbacks, (Action<IRequest, object>[])callbacks);
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
