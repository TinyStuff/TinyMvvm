using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;

namespace TinyMvvmSample.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {

        }

        public ICommand Contact => new TinyCommand(() =>
        {
            Navigation.NavigateToAsync($"{nameof(ContactViewModel)}?id=1");
        });

        public ICommand About => new TinyCommand(() =>
        {
            Navigation.NavigateToAsync("//login?messageTitle=hej");
        });

        public override Task OnAppearing()
        {
            return base.OnAppearing();
        }
    }
}
