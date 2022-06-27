using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TinyMvvm.Sample.Models;

namespace TinyMvvm.Sample.Services;


//Cities json is fetched from here, https://github.com/lutangar/cities.json.
public class CityService : ICityService
{
    private List<City> cities;

    public async Task<List<City>> Search(string text)
    {
        var lowerText = text.ToLower();

        var cities = await Load();

        var result = cities.Where(x => x.Name.ToLower().Contains(lowerText)).ToList();

        return result;
    }

    public async Task<City> Get(string city)
    {
        var lowerText = city.ToLower();

        var cities = await Load();

        var result = cities.Single(x => x.Name == lowerText);

        return result;
    }

    public Task<List<City>> GetAll()
    {
        return Load();
    }

    private async Task<List<City>> Load()
    {
        if(cities != null)
        {
            return cities;
        }

        using var stream = await FileSystem.OpenAppPackageFileAsync("cities.json");
        using var reader = new StreamReader(stream);

        var contents = await reader.ReadToEndAsync();

        cities = JsonSerializer.Deserialize<List<City>>(contents, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return cities;
    }
}

