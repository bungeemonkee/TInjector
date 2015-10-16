namespace TInjector
{
    /// <summary>
    /// Functionality all DI root objects must implement.
    /// </summary>
    public interface IRoot
    {
        T Get<T>();
    }
}
