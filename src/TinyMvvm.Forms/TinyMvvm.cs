using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TinyMvvm.Forms;

public static class TinyMvvm
{
    [Obsolete("You don't have to run this method anymore!")]
    public static void Initialize()
    {
        var navigation = (FormsNavigationHelper)NavigationHelper.Current;

        navigation.ViewCreator = new TinyMvvmViewCreator();
    }
}
