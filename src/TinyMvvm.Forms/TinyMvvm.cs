using System;
using System.Collections.Generic;
using System.Text;
using TinyNavigationHelper.Abstraction;
using TinyNavigationHelper.Forms;
using Xamarin.Forms;

namespace TinyMvvm.Forms
{
    public static class TinyMvvm
    {
        public static void Initialize()
        {
            var navigation = (FormsNavigationHelper)NavigationHelper.Current;

            navigation.ViewCreator = new TinyMvvmViewCreator();
        }
    }
}
