using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using TinyMvvm.IoC;
using TinyNavigationHelper.Abstraction;
using TinyNavigationHelper.Forms;
using Xamarin.Forms;

namespace TinyMvvm.Forms
{
    public class TinyMvvmViewCreator : IViewCreator<Page>
    {
        public Page Create(Type type)
        {
            return Create(type, null);
        }

        public Page Create(Type type, object parameter)
        {
            Page page = null;

            if (Resolver.IsEnabled)
            {
                page = (Page)Resolver.Resolve(type);

                var view = page as ViewBase;

                if (view != null)
                {
                    view.CreatedByTinyMvvm = true;

                    view.CreateViewModel(); 
                }

                if (view?.BindingContext is ViewModelBase)
                {
                    TinyMvvmSetup(view, parameter);
                }
                else
                {
                    if (ParameterSetter.CanSet(type))
                    {
                        ParameterSetter.Set(page, parameter);
                    }
                }
            }
            else
            {
                var defaultCreator = new DefaultViewCreator();
                page = defaultCreator.Create(type);

                var view = page as ViewBase;

                if (view != null)
                {
                    view.CreatedByTinyMvvm = true;

                    view.CreateViewModel(); 
                }

                if (view?.BindingContext is ViewModelBase)
                {
                    TinyMvvmSetup(view, parameter);
                }
                else
                {
                    if (ParameterSetter.CanSet(type))
                    {
                        ParameterSetter.Set(page, parameter);
                    }
                }
            }

            return page;
        }

        private void TinyMvvmSetup(ViewBase view, object parameter )
        {
            

            if (parameter != null)
            {
                view.NavigationParameter = parameter;
            }

            var viewModel = view.BindingContext as ViewModelBase;

            if (viewModel != null)
            {
                ViewBase.SetupUIAction(viewModel);
                if (parameter != null)
                {
                    viewModel.NavigationParameter = parameter;
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await view.ReadLock.WaitAsync();
                        await viewModel.Initialize();
                    }
                    finally
                    {
                        view.ReadLock.Release();
                    }
                });
            }
        }

       
    }
}
