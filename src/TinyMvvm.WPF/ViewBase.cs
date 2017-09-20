using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TinyMvvm.IoC;

namespace TinyMvvm.WPF
{
    public class ViewBase<T> : Window where T:INotifyPropertyChanged
    {
        public T ViewModel { get; private set; }

        private static readonly SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);

        public ViewBase()
        {
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

                    
                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //    await _readLock.WaitAsync();
                    //    await viewModel.Initialize();
                    //    _readLock.Release();
                    //});
                }
            }
        }
    }
}
