namespace TinyMvvm.Maui;

public class Navigation : INavigation
{
    private Dictionary<string, string> queries = new Dictionary<string, string>();
    private Dictionary<string, object> parameters = new Dictionary<string, object>();

    public Navigation()
    {
        NavigationHelper.Current = this;
    }

    public void RegisterRoute(string route, Type type)
    {
        try
        {

            Routing.RegisterRoute(route, type);
        }
        catch (ArgumentException)
        {
            //Catch to avoid app crash if route already registered
        }
    }

    public async override Task NavigateToAsync(string key)
    {

        if (Views.ContainsKey(key.ToLower()))
        {
            await base.NavigateToAsync(key);
            return;
        }

        if (key.Contains("?"))
        {
            var route = key.Split('?');

            var tinyId = Guid.NewGuid().ToString();
            key = $"{key}&tinyid={tinyId}";

            queries.Add(tinyId, route.Last());
        }

        await Shell.Current.GoToAsync(key);

    }

    public async override Task NavigateToAsync(string key, object parameter)
    {
        try
        {
            if (Views.ContainsKey(key.ToLower()))
            {
                await base.NavigateToAsync(key, parameter);
                return;
            }

            var tinyId = Guid.NewGuid().ToString();

            if (key.Contains("?"))
            {
                var route = key.Split('?');

                key = $"{key}&tinyid={tinyId}";

                queries.Add(tinyId, route.Last());
            }
            else
            {
                key = $"{key}?tinyid={tinyId}";
            }

            parameters.Add(tinyId, parameter);

            await Shell.Current.GoToAsync(key);
        }
        catch (Exception)
        {
            await base.NavigateToAsync(key);
        }
    }

    /// <summary>
    /// Register all viewModels in a assembly so you can use them to navigate.
    /// </summary>
    /// <param name="viewModelAssembly"></param>
    public void InitViewModelNavigation(Assembly viewModelAssembly)
    {

        foreach (var type in viewModelAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(Page))))
        {
            if (type.GenericTypeArguments.Length > 0)
            {
                RegisterRoute(type.GenericTypeArguments[0].Name, type);
            }

            if (type.BaseType.GenericTypeArguments.Length > 0)
            {
                RegisterRoute(type.BaseType.GenericTypeArguments[0].Name, type);
            }

        }
    }
}
