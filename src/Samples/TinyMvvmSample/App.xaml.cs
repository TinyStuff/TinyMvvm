using System;
using System.Reflection;
using TinyMvvmSample.Views;
using TinyNavigationHelper.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinyMvvmSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationHelper = new ShellNavigationHelper();
           //navigationHelper.RegisterRoute("about/contact", typeof(ContactView));

            navigationHelper.RegisterViewsInAssembly(Assembly.GetExecutingAssembly());

            
            TinyMvvm.Forms.TinyMvvm.Initialize();

            MainPage = new MainShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
