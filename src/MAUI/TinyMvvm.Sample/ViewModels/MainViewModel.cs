using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TinyMvvm.Sample.Models;
using TinyMvvm.Sample.Services;

namespace TinyMvvm.Sample.ViewModels;

public class MainViewModel : TinyViewModel
{
    private readonly ICityService cityService;

    public MainViewModel(ICityService cityService)
    {
        this.cityService = cityService;
    }

    private string text;
    public string Text
    {
        get => text;
        set => Set(ref text, value);
    }

    private ICommand search;
    public ICommand Search => search ??= new TinyCommand(async () =>
    {
        IsBusy = true;

        var result = await cityService.Search(Text);

        Cities = new ObservableCollection<City>(result);

        IsBusy = false;
    });

    private ICommand show;
    public ICommand Show => show ??= new TinyCommand<City>(async (city) =>
    {
        await Navigation.NavigateTo(nameof(DetailsViewModel), city);
    });

    private ObservableCollection<City> cities = new ObservableCollection<City>();
    public ObservableCollection<City> Cities
    {
        get => cities;
        set => Set(ref cities, value);
    }
}

