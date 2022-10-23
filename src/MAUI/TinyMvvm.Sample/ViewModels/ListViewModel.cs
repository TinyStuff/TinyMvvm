using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TinyMvvm.Sample.Models;
using TinyMvvm.Sample.Services;

namespace TinyMvvm.Sample.ViewModels
{
    public partial class ListViewModel : TinyViewModel
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
            Cities = new ObservableCollection<City>(result.Take(100));

            IsBusy = false;
        }

        [ObservableProperty]
        private ObservableCollection<City> cities = new ObservableCollection<City>();

        private ICommand show;
        public ICommand Show => show ??= new RelayCommand<City>(async (city) =>
        {
            await Navigation.NavigateTo($"{nameof(DetailsViewModel)}?city={city.Name}");
        });
    }
}

