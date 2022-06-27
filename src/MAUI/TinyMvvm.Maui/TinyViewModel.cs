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
    public virtual Task ParameterSet() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnAppearing()
    {

        if (!hasAppeared)
        {

            MainThread.BeginInvokeOnMainThread(async () => await OnFirstAppear());

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
            var updated = Set(ref _isBusy, value);

            if (updated)
            {
                RaisePropertyChanged(nameof(IsNotBusy));
            }
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


    private bool _isInitialized;
    /// <inheritdoc />
    public bool IsInitialized
    {
        get { return _isInitialized; }
        set
        {
            var updated = Set(ref _isInitialized, value);

            if (updated)
            {
                RaisePropertyChanged("IsNotInitialized");
            }
        }
    }

    /// <inheritdoc />
    public bool IsNotInitialized
    {
        get
        {
            return !IsInitialized;
        }
    }

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
    protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, newValue))
        {
            field = newValue;
            RaisePropertyChanged(propertyName);

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query == null || query.Count == 0)
        {
            return;
        }

        if (query.ContainsKey("tinyParameter"))
        {
            NavigationParameter = query["tinyParameter"];
        }
        else
        {
            QueryParameters = query;
        }

        async Task RunParameterSet()
        {
            await ParameterSet();
        }

        if (MainThread.IsMainThread)
        {
            await RunParameterSet();
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await RunParameterSet();
            });
        }
    }
}