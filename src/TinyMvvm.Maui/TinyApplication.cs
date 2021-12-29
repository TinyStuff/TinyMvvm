namespace TinyMvvm.Maui;

/// <summary>
/// Subclass of <see cref="Application"/> that enables more features for TinyMvvm.
/// </summary>
public abstract class TinyApplication : Application, ITinyApplication
{
    private WeakEventManager eventManager = new();

    event EventHandler ITinyApplication.ApplicationResume
    {
        add => eventManager.AddEventHandler(nameof(ITinyApplication.ApplicationResume), value);
        remove => eventManager.RemoveEventHandler(nameof(ITinyApplication.ApplicationResume), value);
    }

    event EventHandler ITinyApplication.ApplicationSleep
    {
        add => eventManager.AddEventHandler(nameof(ITinyApplication.ApplicationSleep), value);
        remove => eventManager.RemoveEventHandler(nameof(ITinyApplication.ApplicationSleep), value);
    }

    public TinyApplication()
    {
        ApplicationResolver.Current = this;
    }

    protected override void OnResume()
    {
        base.OnResume();

        eventManager.HandleEvent(this, EventArgs.Empty, nameof(ITinyApplication.ApplicationResume));
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        eventManager.HandleEvent(this, EventArgs.Empty, nameof(ITinyApplication.ApplicationSleep));
    }
}

