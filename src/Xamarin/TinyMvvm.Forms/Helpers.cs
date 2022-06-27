using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;



namespace TinyMvvm.Forms;

internal static class Helpers
{
    public static Action<Action> BeginInvokeOnMainThread => Device.BeginInvokeOnMainThread;
}
