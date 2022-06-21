namespace TinyMvvm;

public interface INavigation
{
    
    Task NavigateToAsync(string key);
    Task NavigateToAsync(string key, object parameter);
}
