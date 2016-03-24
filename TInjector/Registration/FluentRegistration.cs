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

        public string CreationStackTrace => _parent?.CreationStackTrace ?? _creationStackTrace;

        public IFactory<T> Factory => _parent?.Factory ?? _factory;

        public Type Implementer => _parent?.Implementer ?? _implementer;

        public Scope Scope => _parent?.Scope ?? _scope;

        public Type[] Services => _parent?.Services ?? _services;

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
                // HACK: This is very non-ideal, but protable libraries don't (currently) support direct stack traces
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
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.Length == 0) return;

            if (_parent != null)
            {
                // Add the services to the parent instead
                _parent.AddServices(services);
                return;
            }

            // Resize the new array to be able to hold both
            Array.Resize(ref services, services.Length + _services.Length);

            // Add the current array to the new array
            Array.Copy(_services, 0, services, services.Length - _services.Length, _services.Length);

            // Save the new services array
            _services = services;
        }
    }
}
