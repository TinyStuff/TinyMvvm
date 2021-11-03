using System.Reflection;
using Autofac;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using TinyMvvm.Autofac;
using Application = Microsoft.Maui.Controls.Application;

namespace MauiSample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        var currentAssembly = Assembly.GetExecutingAssembly();

        var navigationHelper = new ShellNavigationHelper();

        navigationHelper.InitViewModelNavigation(currentAssembly);

        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterInstance<INavigationHelper>(navigationHelper);

        var appAssembly = typeof(App).GetTypeInfo().Assembly;
        containerBuilder.RegisterAssemblyTypes(appAssembly)
               .Where(x => x.IsSubclassOf(typeof(Page)));

        containerBuilder.RegisterAssemblyTypes(appAssembly)
               .Where(x => x.IsSubclassOf(typeof(ViewModelBase)));

        var container = containerBuilder.Build();

        Resolver.SetResolver(new AutofacResolver(container));

        MainPage = new AppShell();
    }
}
