namespace TinyMvvm;

public interface IViewModelBase : INotifyPropertyChanged
{
    bool IsInitialized { get; set; }

    Task Initialize();

    Task Returning();

    Task OnAppearing();

    Task OnFirstAppear();

    Task OnDisappearing();

    Task OnApplicationResume();
    Task OnApplicationSleep();

    object? ReturningParameter { get; set; }
    object? NavigationParameter { get; set; }
    Dictionary<string, string>? QueryParameters { get; set; }
}
