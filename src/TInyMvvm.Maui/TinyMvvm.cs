﻿namespace TinyMvvm.Maui;

public static class TinyMvvm
{
    [Obsolete("You don't have to run this method anymore!")]
    public static void Initialize()
    {
        var navigation = (FormsNavigationHelper)NavigationHelper.Current;

        navigation.ViewCreator = new TinyMvvmViewCreator();
    }
}
