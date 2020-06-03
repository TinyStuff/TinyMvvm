using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;

namespace TinyMvvmSample.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand Contact => new TinyCommand(() =>
        {
            var list = new List<string>()
            {
                "hej",
                "tja"
            };

            Navigation.NavigateToAsync($"{nameof(ContactViewModel)}?id=1", list);
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
