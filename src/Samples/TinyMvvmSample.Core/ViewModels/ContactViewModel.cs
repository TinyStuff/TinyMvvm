using System;
using System.Threading.Tasks;
using TinyMvvm;

namespace TinyMvvmSample.Core.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        public ContactViewModel()
        {
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            var parameters = NavigationParameter;
        }

        public override Task OnAppearing()
        {
            return base.OnAppearing();
        }

        public override Task OnDisappearing()
        {
            return base.OnDisappearing();
        }

        public override Task OnFirstAppear()
        {
            return base.OnFirstAppear();
        }
    }
}
