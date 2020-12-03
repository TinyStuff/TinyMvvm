using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

[assembly: InternalsVisibleTo("TinyMvvm.Forms")]
namespace TinyMvvm
{
    public abstract class ViewModelBase : IViewModelBase
    {
        private bool hasAppeared;

        public ViewModelBase()
        {
            if (InvokeOnMainThread == null)
            {
                try
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "TinyMvvm.Forms");

                    if (assembly != null)
                    {
                        var deviceType = assembly.GetType("TinyMvvm.Forms.Helpers");
                        var properties = deviceType.GetProperties();

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


        public virtual Task Initialize() => Task.CompletedTask;


        public virtual Task Returning() => Task.CompletedTask;


        public virtual Task OnAppearing()
        {            

            if(!hasAppeared)
            {
                if(InvokeOnMainThread == null)
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
                        InvokeOnMainThread?.Invoke(async() => await Returning());
                    }
                }
            }

            hasAppeared = true;

            return Task.CompletedTask;
        }


        public virtual Task OnFirstAppear() => Task.CompletedTask;

        public virtual Task OnDisappearing() => Task.CompletedTask;

        public object? ReturningParameter { get; set; }
        public object? NavigationParameter { get; set; }
        public Dictionary<string, string>? QueryParameters { get; set; }

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
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsNotBusy");
            }
        }

        public bool IsNotBusy
        {
            get
            {
                return !IsBusy;
            }
        }

        public bool IsInitialized { get; set; }
     
        internal bool ReturningHasRun { get; set; }

        public void RaisePropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                RaisePropertyChanged(propertyName);
            }
        }
    }
}
