using System;
using System.Reflection;
using Autofac;

namespace TinyMvvm.Forms.Sample
{
    public static class Bootstrapper
    {
        public static void Initialize(ContainerBuilder builder)
        {

			// Views
			var asm = typeof(App).GetTypeInfo().Assembly;
			builder.RegisterAssemblyTypes(asm)
				   .Where(x => x.Name.EndsWith("View", StringComparison.Ordinal));

			// ViewModels
			builder.RegisterAssemblyTypes(asm)
				   .Where(x => x.Name.EndsWith("ViewModel", StringComparison.Ordinal));
        }
    }
}
