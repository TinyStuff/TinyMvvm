using Autofac;
using System;

namespace TinyMvvm.Autofac;

public class AutofacResolver : IResolver
{
    private IContainer _container;

    public AutofacResolver(IContainer container)
    {
        _container = container;
    }

    public T Resolve<T>() where T : class
    {
        return _container.Resolve<T>();
    }

    public T Resolve<T>(string key) where T : class
    {
        return _container.ResolveKeyed<T>(key);
    }

    public object Resolve(Type type)
    {
        return _container.Resolve(type);
    }

    public bool TryResolve<T>(out T resolvedObject) where T : class
    {
        return _container.TryResolve<T>(out resolvedObject);
    }


    public bool TryResolve(Type type, out object resolvedObject)
    {
        return _container.TryResolve(type, out resolvedObject);
    }

}
