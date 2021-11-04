using System;
using Microsoft.Extensions.DependencyInjection;

namespace TinyMvvm.Maui
{
    public class ServiceProviderResolver : IResolver
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T Resolve<T>() where T : class
        {
            var instance = serviceProvider.GetService<T>();

            if(instance == null)
            {
                throw new TypeInitializationException(typeof(T).FullName, null);
            }

            return instance;
        }

        public T Resolve<T>(string key) where T : class
        {
            throw new NotSupportedException("Resolving with key is not supported in this provider");
        }

        public object Resolve(Type type)
        {
            var instance = serviceProvider.GetService(type);

            if (instance == null)
            {
                throw new TypeInitializationException(type.FullName, null);
            }

            return instance;
        }

        public bool TryResolve<T>(out T resolvedObject) where T : class
        {
            var instance = serviceProvider.GetService<T>();

            if (instance == null)
            {
                resolvedObject = default!;
                return false;
            }

            resolvedObject = instance;
            return true;
        }

        public bool TryResolve(Type type, out object resolvedObject)
        {
            var instance = serviceProvider.GetService(type);

            if (instance == null)
            {
                resolvedObject = default!;
                return false;
            }

            resolvedObject = instance;
            return true;
        }
    }
}

