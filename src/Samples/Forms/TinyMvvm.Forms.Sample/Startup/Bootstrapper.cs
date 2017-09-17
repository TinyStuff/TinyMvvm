using System;
using System.Reflection;
using Autofac;
using TinyNavigationHelper;

namespace TinyMvvm.Forms.Sample.Startup
{
    public static class Bootstrapper
    {
        public static void Initialize(App app)
        {
			var builder = new ContainerBuilder();
           
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

			// Build and set
			var container = builder.Build();
			var resolver = new TinyMvvm.Autofac.AutofacResolver(container);
			TinyMvvm.IoC.Resolver.SetResolver(resolver);

            // Platform specifics
			Platform?.Initialize(app, builder);
		}

        public static IBootstrapper Platform { get; set; }
    }
}
