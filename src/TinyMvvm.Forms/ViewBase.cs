using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TinyMvvm.IoC;
using TinyNavigationHelper.Abstraction;
using TinyNavigationHelper.Forms;
using Xamarin.Forms;

namespace TinyMvvm.Forms
{
    public abstract class ViewBase : ContentPage
    {
        public object NavigationParameter { get; set; }
        internal SemaphoreSlim ReadLock { get; private set; } = new SemaphoreSlim(1, 1);
    }

    public abstract class ViewBase<T> : ViewBase where T:INotifyPropertyChanged
    {
        public T ViewModel
        {
            get
            {
                if (BindingContext != null)
                {
                    return (T)BindingContext;
                }

                return default(T);
            }
        }

        

        

        public ViewBase()
        {

            var navigation = (FormsNavigationHelper)NavigationHelper.Current;

            if(!(navigation.ViewCreator is TinyMvvmViewCreator))
            {
                throw new Exception("You must run TinyMvvm.Initialize();");
            }

            if (Resolver.IsEnabled)
            { 
                BindingContext = Resolver.Resolve<T>(); ;
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
                        await ReadLock.WaitAsync();
                        await viewModel.OnAppearing();
                        ReadLock.Release();
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
