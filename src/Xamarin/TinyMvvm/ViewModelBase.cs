namespace TinyMvvm;

/// <summary>
/// Base class for ViewModels. 
/// </summary>
public abstract class ViewModelBase : IViewModelBase
{
    private bool hasAppeared;

    public ViewModelBase()
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

        if (InvokeOnMainThread == null)
        {
            try
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "TinyMvvm.Forms");

                if (assembly != null)
                {
                    var deviceType = assembly.GetType("TinyMvvm.Forms.Helpers");
                    var properties = deviceType!.GetProperties();

                    foreach (var property in properties)
                    {

                        if (property.Name == "BeginInvokeOnMainThread" && property is not null)
                        {
                            InvokeOnMainThread = property.GetValue(null, null) as Action<Action>;
                            break;
                        }
                    }


                }
            }
            catch (Exception)
            {

            }
        }
    }

    [Obsolete("The recommendation is to use MainThread from Xamarin.Essentials instead.")]
    public virtual Action<Action>? BeginInvokeOnMainThread { get; set; }

    private static Action<Action>? InvokeOnMainThread { get; set; }


    /// <inheritdoc />
    public virtual Task Initialize() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Returning() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnAppearing()
    {

        if (!hasAppeared)
        {
            if (InvokeOnMainThread == null)
            {
                _ = Task.Run(async () => await OnFirstAppear());
            }
            else
            {
                InvokeOnMainThread?.Invoke(() => OnFirstAppear());
            }
        }
        else
        {
            if (ReturningHasRun)
            {
                ReturningHasRun = false;
            }
            else
            {
                if (InvokeOnMainThread == null)
                {
                    _ = Task.Run(async () => await Returning());
                }
                else
                {
                    InvokeOnMainThread?.Invoke(async () => await Returning());
                }
            }
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
    public Dictionary<string, string>? QueryParameters { get; set; }

    /// <summary>
    /// Command to use if you want to define how to navigate in the view.
    /// </summary>
    public ICommand NavigateTo
    {
        get
        {
            return new TinyCommand<string>(async (key) =>
            {
                await Navigation.NavigateToAsync(key);
            });
        }
    }

    /// <summary>
    /// Command to use if you want to define how to open a modal in the view.
    /// </summary>
    public ICommand OpenModal
    {
        get
        {
            return new TinyCommand<string>(async (key) =>
            {
                await Navigation.OpenModalAsync(key);
            });
        }
    }

    /// <summary>
    /// Gets the navigation helper.
    /// </summary>
    public INavigationHelper Navigation
    {
        get
        {
            if (NavigationHelper.Current != null)
            {
                return NavigationHelper.Current;
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
    public event PropertyChangedEventHandler PropertyChanged;

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
}