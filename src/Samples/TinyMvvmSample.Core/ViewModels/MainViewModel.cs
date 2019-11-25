using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;

namespace TinyMvvmSample.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand Contact => new TinyCommand(() =>
        {
            Navigation.NavigateToAsync("///about/contact?id=1");
        });

        public ICommand About => new TinyCommand(() =>
        {
            Navigation.NavigateToAsync("///about");
        });

        public override Task OnAppearing()
        {
            return base.OnAppearing();
        }
    }
}
