using System;
using TinyMvvm;
using TinyMvvm.Forms;
using Xamarin.Forms;

namespace ClassicSampleApp.Views
{
    public class MainTabbedView : TabbedPage
    {
        public MainTabbedView()
        {

            var viewCreator = (TinyMvvmViewCreator)((FormsNavigationHelper)NavigationHelper.Current).ViewCreator;

            var mainView = viewCreator.Create(typeof(MainView));
            var aboutView = viewCreator.Create(typeof(AboutView));

            var navigationPage = new NavigationPage(mainView);
            var aboutNavigation = new NavigationPage(aboutView);

            //The title for the tabs.
            navigationPage.Title = "Welcome";

            Children.Add(navigationPage);
            Children.Add(aboutNavigation);
        }
    }
}
