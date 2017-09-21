using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TinyMvvm.IoC;

namespace TinyMvvm.WPF
{
    public class ViewBase<T> : Page where T:INotifyPropertyChanged
    {
        public T ViewModel { get; private set; }

        private static readonly SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);

        public ViewBase()
        {
            NavigationService.Navigated += NavigationService_Navigated;

            if (Resolver.IsEnabled)
            {
                ViewModel = Resolver.Resolve<T>();

                DataContext = ViewModel;
            }

            if (ViewModel is ViewModelBase)
            {
                var viewModel = ViewModel as ViewModelBase;

                if (viewModel != null)
                {
                    Application.Current.Dispatcher.Invoke(async() =>
                    {
                        try
                        {
                            await _readLock.WaitAsync();
                            await viewModel.Initialize();
                        }
                        finally
                        {
                            _readLock.Release();
                        }
                    });                   
                }
            }         
        }

        private void NavigationService_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationService.Navigated -= NavigationService_Navigated;
            NavigationService.Navigating += NavigationService_Navigating;

            if (ViewModel is ViewModelBase)
            {
                var viewModel = ViewModel as ViewModelBase;

                if (viewModel != null)
                {
                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        await _readLock.WaitAsync();
                        await viewModel.OnAppearing();
                        _readLock.Release();
                    });
                }
            }
        }

        private void NavigationService_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            NavigationService.Navigating -= NavigationService_Navigating;

            if (ViewModel is ViewModelBase)
            {
                var viewModel = ViewModel as ViewModelBase;

                if (viewModel != null)
                {
                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        await viewModel.OnDisappearing();
                    });
                }
            }
        }
    }
}
