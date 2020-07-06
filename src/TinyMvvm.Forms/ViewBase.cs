using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TinyMvvm.IoC;
using TinyNavigationHelper.Abstraction;
using TinyNavigationHelper.Forms;
using Xamarin.Forms;

namespace TinyMvvm.Forms
{
    [QueryProperty("TinyId", "tinyid")]
    public abstract class ViewBase : ContentPage
    {
        internal bool CreatedByTinyMvvm { get; set; }
        public object? NavigationParameter { get; set; }
        public Dictionary<string, string>? QueryParameters { get; private set; }
        internal SemaphoreSlim ReadLock { get; private set; } = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Internally used by TinyMvvm
        /// </summary>
        public string? TinyId { get; set; }

        public ViewBase()
        {

        }

        protected async override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (!CreatedByTinyMvvm && BindingContext is ViewModelBase viewModel)
            {
                SetupUIAction(viewModel);
                try
                {
                    if (TinyId != null)
                    {
                        var shellNavigationHelper = (ShellNavigationHelper)NavigationHelper.Current;

                        var queryParameters = shellNavigationHelper.GetQueryParameters(TinyId);

                        if (queryParameters != null)
                        {
                            QueryParameters = queryParameters;
                            viewModel.QueryParameters = queryParameters;
                        }

                        var parameters = shellNavigationHelper.GetParameter(TinyId);

                        if (parameters != null)
                        {
                            viewModel.NavigationParameter = parameters;
                            NavigationParameter = parameters;
                        }
                    }

                    await ReadLock.WaitAsync();
                    await viewModel.Initialize();
                }
                finally
                {
                    ReadLock.Release();
                }
            }
        }

        private bool _hasAppeared;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ViewModelBase viewModel)
            {
                if (TinyId != null)
                {
                    var shellNavigationHelper = (ShellNavigationHelper)NavigationHelper.Current;

                    var queryParameters = shellNavigationHelper.GetQueryParameters(TinyId);

                    if (queryParameters != null)
                    {
                        viewModel.QueryParameters = queryParameters;
                        QueryParameters = queryParameters;
                    }

                    var parameters = shellNavigationHelper.GetParameter(TinyId);

                    if (parameters != null)
                    {
                        viewModel.NavigationParameter = parameters;
                        NavigationParameter = parameters;
                    }
                }

                if (viewModel != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ReadLock.WaitAsync();
                        await viewModel.OnAppearing();

                        if (!_hasAppeared)
                        {
                            _hasAppeared = true;
                            await viewModel.OnFirstAppear();
                        }

                        ReadLock.Release();
                    });
                }
            }
        }

        internal static void SetupUIAction(ViewModelBase viewModel)
        {
            if (viewModel.BeginInvokeOnMainThread == null)
            {
                viewModel.BeginInvokeOnMainThread = (action) =>
                {
                    Device.BeginInvokeOnMainThread(action);
                };
            }
        }

        internal virtual void CreateViewModel()
        {

        }
    }


    
    public abstract class ViewBase<T> : ViewBase where T:ViewModelBase?
    {
        public T ViewModel
        {
            get
            {
                if (BindingContext != null)
                {
                    return (T)BindingContext;
                }

                return null;
            }
        }

        

        internal override void CreateViewModel()
        {
            if (Resolver.IsEnabled)
            {
                BindingContext = Resolver.Resolve<T>();
            }
            else
            {
                BindingContext = Activator.CreateInstance(typeof(T));
            }

            
            
        }

        

        public ViewBase()
        {

            var navigation = (FormsNavigationHelper)NavigationHelper.Current;

            if(!(navigation.ViewCreator is TinyMvvmViewCreator))
            {
                throw new Exception("You must run TinyMvvm.Initialize();");
            }
        }

      
        protected override void OnAppearing()
        {
           

            if(ViewModel == null && !CreatedByTinyMvvm)
            {
                CreateViewModel();
            }

            base.OnAppearing();

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
