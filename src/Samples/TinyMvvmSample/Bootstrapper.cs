using System;
using System.Reflection;
using Autofac;
using TinyNavigationHelper;
using Xamarin.Forms;
using TinyMvvm.Autofac;
using TinyMvvm.IoC;
using TinyMvvmSample.Core.Services;

namespace TinyMvvmSample
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            // Views
            var appAssembly = typeof(App).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(appAssembly)
                   .Where(x => x.Name.EndsWith("View", StringComparison.Ordinal));

            // ViewModels
            var coreAssembly = Assembly.Load(new AssemblyName("TinyMvvmSample.Core"));
            builder.RegisterAssemblyTypes(coreAssembly)
                   .Where(x => x.Name.EndsWith("ViewModel", StringComparison.Ordinal));

            // Navigation
            var navigationHelper = new TinyNavigationHelper.Forms.FormsNavigationHelper(Application.Current);
            navigationHelper.RegisterViewsInAssembly(appAssembly);
            builder.RegisterInstance<INavigationHelper>(navigationHelper);

            builder.RegisterType<NewsService>().As<INewsService>();

            var container = builder.Build();

            var autofacResolver = new AutofacResolver(container);

            Resolver.SetResolver(autofacResolver);

            TinyMvvm.Forms.TinyMvvm.Initialize();

            navigationHelper.SetRootView("MainView");
        }
    }
}
