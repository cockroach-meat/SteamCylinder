namespace SteamCylinder.Core;

public sealed class ServiceLocator
{
    private Dictionary<Type, Type> _services = new();
    private Dictionary<Type, object> _singletonServices = new();

    public ServiceLocator AddService<TContract, TImplementation>() where TContract: IService
    {
        _services.Add(typeof(TContract), typeof(TImplementation));
        return this;
    }

    public ServiceLocator AddSingletonService<T>(T implementation) where T: notnull, IService
    {
        _singletonServices.Add(typeof(T), implementation);
        return this;
    }

    public T Get<T>() where T: IService
    {
        if (_singletonServices.ContainsKey(typeof(T)))
        {
            return (T)_singletonServices[typeof(T)];
        }

        if (_services.ContainsKey(typeof(T)))
        {
            T service = Activator.CreateInstance<T>();
            service.Initialize(this);
            return service;
        }

        throw new KeyNotFoundException($"No service with name \"{typeof(T).Name}\" found");
    }

    public ServiceLocator InitializeSingletonServices()
    {
        foreach (object service in _singletonServices.Values)
        {
            ((IService)service).Initialize(this);
        }

        return this;
    }
}
