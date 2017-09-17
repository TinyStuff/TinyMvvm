using TinyMvvm.Forms.Sample.Startup;
using TinyMvvm.Forms.Sample.Views;
using TinyMvvm.IoC;
using Xamarin.Forms;

namespace TinyMvvm.Forms.Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Bootstrapper.Initialize(this);
            MainPage = new NavigationPage(Resolver.Resolve<MainView>());
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
