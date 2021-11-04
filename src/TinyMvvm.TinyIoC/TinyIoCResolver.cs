using System;
using TinyIoC;
using TinyMvvm;

namespace TinyMvvm.TinyIoC
{
    public class TinyIoCResolver : IResolver
    {
        public T Resolve<T>() where T : class
        {
            return (T)TinyIoCContainer.Current.Resolve<T>();
        }

        public T Resolve<T>(string key) where T:class
        {
            return TinyIoCContainer.Current.Resolve<T>(key);
        }

        public object Resolve(Type type)
        {
            return TinyIoCContainer.Current.Resolve(type);
        }

        public bool TryResolve<T>(out T resolvedObject) where T : class
        {
            return TinyIoCContainer.Current.TryResolve<T>(out resolvedObject);
        }

        public bool TryResolve<T>(string key, out T resolvedObject) where T : class
        {
            return TinyIoCContainer.Current.TryResolve<T>(key, out resolvedObject);
        }

        public bool TryResolve(Type type, out object resolvedObject)
        {
            return TinyIoCContainer.Current.TryResolve(type, out resolvedObject);
        }

    }
}
