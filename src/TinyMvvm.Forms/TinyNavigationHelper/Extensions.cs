using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TinyNavigationHelper.Forms
{
    public static class Extensions
    {
        public static async Task NavigateToAsync(this INavigationHelper helper, Page page)
        {
            var formsHelper = helper as FormsNavigationHelper;
            if (formsHelper == null)
            {
                throw new ArgumentException("This extension only works in Xamarin Forms");
            }

            await formsHelper.NavigateToAsync(page);
        }

        public static async Task OpenModalAsync(this INavigationHelper helper, Page page, bool withNavigation = false)
        {
            var formsHelper = helper as FormsNavigationHelper;
            if (formsHelper == null)
            {
                throw new ArgumentException("This extension only works in Xamarin Forms");
            }

            await formsHelper.OpenModalAsync(page, withNavigation);
        }

        public static async Task NavigateToAsync<T>(this INavigationHelper helper)
        {
            var viewType = typeof(T);
            var viewName = viewType.Name;

            await helper.NavigateToAsync(viewName);
        }

        public static async Task NavigateToAsync<T>(this INavigationHelper helper, object parameter)
        {
            var viewType = typeof(T);
            var viewName = viewType.Name;

            await helper.NavigateToAsync(viewName, parameter);
        }
    }
}
