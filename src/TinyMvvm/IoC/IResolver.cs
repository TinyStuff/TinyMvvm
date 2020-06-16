using System;
using System.Collections.Generic;
using System.Text;

namespace TinyMvvm.IoC
{
    public interface IResolver
    {
        T Resolve<T>();
        T Resolve<T>(string key);
        object Resolve(Type type);
    }
}
