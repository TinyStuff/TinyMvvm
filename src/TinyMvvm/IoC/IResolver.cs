using System;
using System.Collections.Generic;
using System.Text;

namespace TinyMvvm.IoC
{
    public interface IResolver
    {
        T Resolve<T>() where T : class;
        T Resolve<T>(string key) where T : class;
        object Resolve(Type type);
        bool TryResolve<T>(out T resolvedObject) where T : class;
        bool TryResolve(Type type, out object resolvedObject);
    }
}
