using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm.IoC;
using TinyNavigationHelper;
using TinyNavigationHelper.Abstraction;

namespace TinyMvvm
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {

        }

        [Obsolete("The recommendation is to use MainThread from Xamarin.Essentials instead.")]
        public virtual Action<Action>? BeginInvokeOnMainThread { get; set; }


        public async virtual Task Initialize()
        {

        }

        public async virtual Task OnAppearing()
        {

        }

        public async virtual Task OnFirstAppear()
        {

        }

        public async virtual Task OnDisappearing()
        {

        }

        public object? NavigationParameter { get; set; }

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
