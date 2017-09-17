using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TinyMvvm.IoC;
using Xamarin.Forms;

namespace TinyMvvm.Forms
{
 
    public class ViewBase<T> : ContentPage where T:INotifyPropertyChanged
    {
        public T ViewModel { get; private set; }

        private static readonly SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);

        public ViewBase()
        {
            if (Resolver.IsEnabled)
            {
                ViewModel = Resolver.Resolve<T>();

                BindingContext = ViewModel;
            }
            
            if(ViewModel is ViewModelBase)
            {
                var viewModel = ViewModel as ViewModelBase;

                if (viewModel != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await _readLock.WaitAsync();
                        await viewModel.Initialize();
                        _readLock.Release();
                    }); 
                }            
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel is ViewModelBase)
            {
                var viewModel = ViewModel as ViewModelBase;

                if (viewModel != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await _readLock.WaitAsync();
                        await viewModel.OnAppearing();
                        _readLock.Release();
                    });
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (ViewModel is ViewModelBase)
            {
                var viewModel = ViewModel as ViewModelBase;

                if (viewModel != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await viewModel.OnDisappearing();
                    });
                }
            }
        }
    }
}
