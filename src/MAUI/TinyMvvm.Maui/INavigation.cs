namespace TinyMvvm;

public interface INavigation
{
    
    Task NavigateTo(string key);
    Task NavigateTo(string key, object parameter);
}
