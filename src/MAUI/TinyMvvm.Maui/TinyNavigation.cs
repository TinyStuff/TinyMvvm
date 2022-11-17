using System.Collections;

namespace TinyMvvm;

public class TinyNavigation : INavigation
{
    public static INavigation Current { get; set; } = null!;

    public TinyNavigation()
    {
        Current = this;
    }

    /// <inheritdoc />
    public async Task NavigateTo(string key)
    {
        await Shell.Current.GoToAsync(key);

    }

    /// <inheritdoc />
    public async Task NavigateTo(string key, object parameter)
    {
        if (parameter is IDictionary<string, object> dictionary)
        {
            await Shell.Current.GoToAsync(key, dictionary);
            return;
        }

        var parameters = new Dictionary<string, object>();
        parameters.Add("tinyParameter", parameter);

        await Shell.Current.GoToAsync(key, parameters);
    }
}
