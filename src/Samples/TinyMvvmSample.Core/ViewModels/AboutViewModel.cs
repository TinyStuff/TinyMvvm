using System;
using System.Threading.Tasks;
using TinyMvvm;

namespace TinyMvvmSample.Core.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
        }

        public override Task OnAppearing()
        {
            return base.OnAppearing();
        }
    }
}
