using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TinyMvvm;

/// <summary>
/// Base class for ViewModels. 
/// </summary>
public abstract class TinyViewModel : ITinyViewModel, IQueryAttributable
{
    private bool hasAppeared;

    public TinyViewModel()
    {
        var application = ApplicationResolver.Current;

        if (application != null)
        {

            application.ApplicationResume += (sender, args) =>
            {
                OnApplicationResume();
            };

            application.ApplicationSleep += (sender, args) =>
            {
                OnApplicationSleep();
            };
        }
    }

    /// <inheritdoc />
    public virtual Task Initialize() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Returning() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnAppearing()
    {

        if (!hasAppeared)
        {

           MainThread.BeginInvokeOnMainThread(async () => await OnFirstAppear());
            
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () => await Returning());
        }

        hasAppeared = true;

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task OnFirstAppear() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnDisappearing() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnApplicationResume() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnApplicationSleep() => Task.CompletedTask;

    /// <inheritdoc />
    public object? ReturningParameter { get; set; }

    /// <inheritdoc />
    public object? NavigationParameter { get; set; }

    /// <inheritdoc />
    public IDictionary<string, object>? QueryParameters { get; set; }


    /// <summary>
    /// Gets the navigation.
    /// </summary>
    public INavigation Navigation
    {
        get
        {
            if (TinyNavigation.Current != null)
            {
                return TinyNavigation.Current;
            }
            else
            {
                throw new Exception("Navigation has not been initialized.");
            }
        }
    }

    private bool _isBusy;
    /// <inheritdoc />
    public bool IsBusy
    {
        get { return _isBusy; }
        set
        {
            _isBusy = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(IsNotBusy));
        }
    }

    /// <inheritdoc />
    public bool IsNotBusy
    {
        get
        {
            return !IsBusy;
        }
    }

    /// <inheritdoc />
    public bool IsInitialized { get; set; }

    internal bool ReturningHasRun { get; set; }

    public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// To be used in properties, will check if value is changed and raise the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="field"></param>
    /// <param name="newValue"></param>
    /// <param name="propertyName"></param>
    protected void Set<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, newValue))
        {
            field = newValue;
            RaisePropertyChanged(propertyName);
        }
    }

    /// <inheritdoc />
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query.ContainsKey("tinyParameter"))
        {
            NavigationParameter = query["tinyParameter"];
        }
        else
        {
            QueryParameters = query;
        }
    }
}