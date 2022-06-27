using System;
using System.Reflection;
using Autofac;
using ClassicSampleApp.Views;
using TinyMvvm;
using TinyMvvm.Autofac;
using TinyMvvm.Forms;
using Xamarin.Forms;

namespace ClassicSampleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var currentAssembly = Assembly.GetExecutingAssembly();

            var navigationHelper = new FormsNavigationHelper();

            navigationHelper.RegisterViewsInAssembly(currentAssembly);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance<INavigationHelper>(navigationHelper);


            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                   .Where(x => x.IsSubclassOf(typeof(Page)));

            containerBuilder.RegisterAssemblyTypes(currentAssembly)
                   .Where(x => x.IsSubclassOf(typeof(ViewModelBase)));

            var container = containerBuilder.Build();

            Resolver.SetResolver(new AutofacResolver(container));

            navigationHelper.SetRootView(nameof(MainTabbedView), false);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
