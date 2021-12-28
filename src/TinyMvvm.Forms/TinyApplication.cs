using System;
using Xamarin.Forms;
using TinyMvvm;

namespace TinyMvvm.Forms;

public abstract class TinyApplication : Application, ITinyApplication
{
    private WeakEventManager eventManager = new();

    event EventHandler IApplicationBase.ApplicationResume
    {
        add => eventManager.AddEventHandler(nameof(IApplicationBase.ApplicationResume), value);
        remove => eventManager.RemoveEventHandler(nameof(IApplicationBase.ApplicationResume), value);
    }

    event EventHandler IApplicationBase.ApplicationSleep
    {
        add => eventManager.AddEventHandler(nameof(IApplicationBase.ApplicationSleep), value);
        remove => eventManager.RemoveEventHandler(nameof(IApplicationBase.ApplicationSleep), value);
    }

    public TinyApplication()
    {
        ApplicationResolver.Current = this;
    }

    protected override void OnResume()
    {
        base.OnResume();

        eventManager.HandleEvent(this, EventArgs.Empty, nameof(IApplicationBase.ApplicationResume));
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        eventManager.HandleEvent(this, EventArgs.Empty, nameof(IApplicationBase.ApplicationSleep));
    }
}

