using Autofac;
using System;
using TinyMvvm.IoC;

namespace TinyMvvm.Autofac
{
    public class AutofacResolver : IResolver
    {
        private IContainer _container;

        public AutofacResolver(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(string key)
        {
            return _container.ResolveKeyed<T>(key);
        }
    }
}
