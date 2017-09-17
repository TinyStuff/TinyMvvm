using System;
using Autofac;

namespace TinyMvvm.Forms.Sample.Startup
{
    public interface IBootstrapper
    {
        void Initialize(App app, ContainerBuilder builder);
    }
}
