using System;
using System.Collections.Generic;
using System.Text;

namespace TinyMvvm.IoC
{
    public static class Resolver
    {
        private static IResolver? _resolver;

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
            if(_resolver is null)
            {
                throw new NullReferenceException("You must run SetResolver before calling resolve");
            }

            return _resolver.Resolve<T>();
        }

        public static T Resolve<T>(string key)
        {
            if (_resolver is null)
            {
                throw new NullReferenceException("You must run SetResolver before calling resolve");
            }

            return _resolver.Resolve<T>(key);
        }

        public static object Resolve(Type type)
        {
            if (_resolver is null)
            {
                throw new NullReferenceException("You must run SetResolver before calling resolve");
            }

            return _resolver.Resolve(type);
        }
    }
}
