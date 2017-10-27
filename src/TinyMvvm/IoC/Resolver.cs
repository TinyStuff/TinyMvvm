using System;
using System.Collections.Generic;
using System.Text;

namespace TinyMvvm.IoC
{
    public static class Resolver
    {
        private static IResolver _resolver;

        public static void SetResolver(IResolver resolver)
        {
            _resolver = resolver;
        }

        public static bool IsEnabled
        {
            get
            {
                return _resolver != null;
            }
        }

        public static T Resolve<T>()
        {
            return _resolver.Resolve<T>();
        }

        public static T Resolve<T>(string key)
        {
            return _resolver.Resolve<T>(key);
        }

        public static object Resolve(Type type)
        {
            return _resolver.Resolver(type);
        }
    }
}
