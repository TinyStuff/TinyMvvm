using System;
namespace TinyMvvm.Maui;

internal static class TinyDispatcher
{
    internal static void BeginInvokeOnMainThread(Action action)
    {
        //Run Task.Run instead, because MainThread is not supported during unit tests.
        if (DeviceInfo.Platform == DevicePlatform.Unknown)
        {
            Task.Run(action);
            return;
        }

        MainThread.BeginInvokeOnMainThread(action);
    }

    internal static bool IsMainThread
    {
        get
        {
            //Always return true, because MainThread is not supported during unit tests.
            if (DeviceInfo.Platform == DevicePlatform.Unknown)
            {
                return true;
            }

            return MainThread.IsMainThread;
        }
    }
}

