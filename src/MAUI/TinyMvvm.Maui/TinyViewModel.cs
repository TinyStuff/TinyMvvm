using CommunityToolkit.Mvvm.ComponentModel;
using TinyMvvm.Maui;

namespace TinyMvvm;

/// <summary>
/// Base class for ViewModels. 
/// </summary>
[INotifyPropertyChanged]
public abstract partial class TinyViewModel : ITinyViewModel, IQueryAttributable
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
    public virtual Task OnParameterSet() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnAppearing()
    {

        if (!hasAppeared)
        {
            TinyDispatcher.BeginInvokeOnMainThread(async () => await OnFirstAppear());
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;


    /// <inheritdoc />
    public bool IsNotBusy
    {
        get
        {
            return !IsBusy;
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotInitialized))]
    private bool _isInitialized;


    /// <inheritdoc />
    public bool IsNotInitialized
    {
        get
        {
            return !IsInitialized;
        }
    }

    internal bool ReturningHasRun { get; set; }

    private const string TinyParameterKey = "tinyParameter";
    /// <inheritdoc />
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0)
        {
            return;
        }

        if (query.ContainsKey(TinyParameterKey))
        {
            NavigationParameter = query[TinyParameterKey];

            if (query.Count > 1)
            {
                var queryParameters = new Dictionary<string, object>();

                foreach (var item in query)
                {
                    if (item.Key == TinyParameterKey)
                    {
                        continue;
                    }

                    queryParameters.Add(item.Key, item.Value);
                }

                QueryParameters = queryParameters;
            }
        }
        else
        {
            QueryParameters = query;
        }

        if (TinyDispatcher.IsMainThread)
        {
            await OnParameterSet();
        }
        else
        {
            TinyDispatcher.BeginInvokeOnMainThread(async () =>
            {
                await OnParameterSet();
            });
        }
    }
}