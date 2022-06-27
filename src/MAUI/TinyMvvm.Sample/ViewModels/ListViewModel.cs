using System.Collections.ObjectModel;
using System.Windows.Input;
using TinyMvvm.Sample.Models;
using TinyMvvm.Sample.Services;

namespace TinyMvvm.Sample.ViewModels
{
	public class ListViewModel : TinyViewModel
	{
        private readonly ICityService cityService;

        public ListViewModel(ICityService cityService)
		{
            this.cityService = cityService;
        }

        public override async Task Initialize()
        {
            IsBusy = true;

            var result = await cityService.GetAll();
            Cities = new ObservableCollection<City>(result);

            IsBusy = false;
        }

        private ObservableCollection<City> cities = new ObservableCollection<City>();
        public ObservableCollection<City> Cities
        {
            get => cities;
            set => Set(ref cities, value);
        }

        private ICommand show;
        public ICommand Show => show ??= new TinyCommand<City>(async (city) =>
        {
            await Navigation.NavigateTo($"{nameof(DetailsViewModel)}?city={city.Name}");
        });
    }
}

