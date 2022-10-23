using CommunityToolkit.Mvvm.ComponentModel;
using TinyMvvm.Sample.Models;
using TinyMvvm.Sample.Services;

namespace TinyMvvm.Sample.ViewModels
{
    public partial class DetailsViewModel : TinyViewModel
	{
        private readonly ICityService cityService;

        public DetailsViewModel(ICityService cityService)
		{
            this.cityService = cityService;
        }

        public override async Task OnParameterSet()
        {
            IsBusy = true;

            if (NavigationParameter is City city)
            {
                City = city;
            }
            else
            {
                var text = QueryParameters["city"];
                City = await cityService.Get(text.ToString());
            }

            IsBusy = false;
        }

        [ObservableProperty]
        private City city;
    }
}

