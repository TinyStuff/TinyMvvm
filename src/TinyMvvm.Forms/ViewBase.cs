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
        internal protected bool isShellView;

        private string? tinyId;
        /// <summary>
        /// Internally used by TinyMvvm
        /// </summary>
        public string? TinyId
        {
            get => tinyId;
            set
            {
                tinyId = value;

                if (TinyId != null && !CreatedByTinyMvvm && BindingContext is IViewModelBase viewModel)
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

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await ReadLock.WaitAsync();
                            await viewModel.Initialize();
                        }
                        finally
                        {
                            ReadLock.Release();
                        }
                    });                   
                   
                }
            }
        }

        public ViewBase()
        {

        }

        protected async override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (!CreatedByTinyMvvm && BindingContext is IViewModelBase viewModel)
            {
                SetupUIAction(viewModel);
                try
                {
                    

                    if (!isShellView)
                    { 
                        await ReadLock.WaitAsync();
                        await viewModel.Initialize();

                        viewModel.IsInitialized = true;
                    }
                }
                finally
                {
                    if (!isShellView)
                    {
                        ReadLock.Release();
                    }
                }
            }
        }

        private bool _hasAppeared;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IViewModelBase viewModel)
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

                        if(!viewModel.IsInitialized)
                        {
                            await viewModel.Initialize();
                        }

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

        internal static void SetupUIAction(IViewModelBase viewModel)
        {
            if (viewModel is ViewModelBase viewModelBase)
            {

                if (viewModelBase.BeginInvokeOnMainThread == null)
                {
                    viewModelBase.BeginInvokeOnMainThread = (action) =>
                    {
                        Device.BeginInvokeOnMainThread(action);
                    };
                }
            }
        }

        internal virtual void CreateViewModel()
        {

        }
    }


    
    public abstract class ViewBase<T> : ViewBase where T:IViewModelBase?
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

        public ViewBase(bool isShellView)
        {
            this.isShellView = isShellView;

            if (isShellView)
            {
                CreateViewModel();
            }
        }

        public ViewBase()
        {

            var navigation = (FormsNavigationHelper)NavigationHelper.Current;

            if(!(navigation.ViewCreator is TinyMvvmViewCreator))
            {
                navigation.ViewCreator = new TinyMvvmViewCreator();
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

            if (ViewModel is IViewModelBase viewModel)
            {
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

    public class ShellViewBase<T> : ViewBase<T> where T:IViewModelBase?
    {
        public ShellViewBase() : base(true)
        {
        }
    }
}
