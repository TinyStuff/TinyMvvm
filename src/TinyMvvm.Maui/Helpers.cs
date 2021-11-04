namespace TinyMvvm.Maui;

internal static class Helpers
{
    public static Action<Action> BeginInvokeOnMainThread => Device.BeginInvokeOnMainThread;
}

