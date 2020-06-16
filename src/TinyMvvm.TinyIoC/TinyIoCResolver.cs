using System;
using TinyIoC;
using TinyMvvm.IoC;

namespace TinyMvvm.TinyIoC
{
    public class TinyIoCResolver : IResolver
    {
        public T Resolve<T>()
        {
            var type = typeof(T);

            return (T)TinyIoCContainer.Current.Resolve(type);
        }

        public T Resolve<T>(string key)
        {
            var type = typeof(T);

            return (T)TinyIoCContainer.Current.Resolve(type, key);
        }

        public object Resolve(Type type)
        {
            return TinyIoCContainer.Current.Resolve(type);
        }
    }
}
