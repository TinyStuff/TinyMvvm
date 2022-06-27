using TinyMvvm.Sample.Models;
using TinyMvvm.Sample.Services;

namespace TinyMvvm.Sample.ViewModels
{
	public class DetailsViewModel : TinyViewModel
	{
		public DetailsViewModel(ICityService cityService)
		{
            this.cityService = cityService;
        }

        public override async Task ParameterSet()
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

        private City city;
        private readonly ICityService cityService;

        public City City
        {
            get => city;
            set => Set(ref city, value);
        }
    }
}

