using System;
using System.Reflection;
using Autofac;
using TinyNavigationHelper;

namespace TinyMvvm.Forms.Sample
{
    public static class Bootstrapper
    {
        public static void Initialize(App app, ContainerBuilder builder)
        {
			// Views
			var asm = typeof(App).GetTypeInfo().Assembly;
			builder.RegisterAssemblyTypes(asm)
				   .Where(x => x.Name.EndsWith("View", StringComparison.Ordinal));

			// ViewModels
			builder.RegisterAssemblyTypes(asm)
				   .Where(x => x.Name.EndsWith("ViewModel", StringComparison.Ordinal));

			// Navigation
			var navigationHelper = new TinyNavigationHelper.Forms.FormsNavigationHelper(app);
			navigationHelper.RegisterViewsInAssembly(asm);
            builder.RegisterInstance<INavigationHelper>(navigationHelper);
		}
    }
}
