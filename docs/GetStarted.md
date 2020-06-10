# TinyMvvm Get Started Tutorial
This is a tutorial that will guide you through how to get started building an app using Xamarin.Forms and TinyMvvm.

1. Create a new project based on the template for a **blank** Xamarin.Forms app. In this example, the name of the project will be **SampleApp**. The template will generate one project for shared code and one project per target platform. 
1. Install the following NuGet packages into all projects:
    * TinyMvvm.Forms
    * TinyMvvm.Autofac
1. Create a new **ContentPage XAML** in the **SampleApp** project and give it the name **AppShell**. And add the following content to it.
    ```xml
    <Shell xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" xmlns:views="clr-namespace:SampleApp.Views" x:Class="SampleApp.AppShell">
        <TabBar>
            <ShellContent Title="Home" Route="home" >
                <views:MainView />
             </ShellContent>
            <ShellContent Title="About" Route="about">
                <views:AboutView />
             </ShellContent>
        </TabBar>
    </Shell>
    ```

1. Remove the base class in the AppShell.xaml.cs so it will look like this:
    ```csharp
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();
        }
    }
    ```
1. Navigate to **App.xaml.cs** and set **MainPage** to **AppShell**. You can also delete the MainPage.xaml- and MainPage.xaml.cs files.
    ```csharp
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
    ```
1. On the rows before you set MainPage to AppShell write the initialization code for TinyMvvm:
    * Use **ShellNavigationHelper** and register all views in the current Assembly. This register all views so we can use the class name as the key if we use the classic (non-Shell) navigation or we can use ViewModelNavigation that is powered by the Shell navigation.
    * The sample is using **Autofac** as it's **IoC container**, but you can use whatever container you want. The only thing you need to do to use another is to create an implementation of **IResolver** that uses it. Register all classes that is a subType of **Page** (Xamarin.Forms) and **ViewModelBase** (TinyMvvm).
    * Register the container to the Resolver, the **Resolver** is used internally by TinyMvvm, but you can also use it in your code.
    * The last thing to do is to call the **Initialize* method for TinyMvvm.
    ```csharp
        public App()
        {
            InitializeComponent();

            var navigationHelper = new ShellNavigationHelper();

            var currentAssembly = Assembly.GetExecutingAssembly();
            navigationHelper.RegisterViewsInAssembly(currentAssembly);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance<INavigationHelper>(navigationHelper);

            var appAssembly = typeof(App).GetTypeInfo().Assembly;
            containerBuilder.RegisterAssemblyTypes(appAssembly)
                   .Where(x => x.IsSubclassOf(typeof(Page)));

            containerBuilder.RegisterAssemblyTypes(appAssembly)
                   .Where(x => x.IsSubclassOf(typeof(ViewModelBase)));

            var container = containerBuilder.Build();

            Resolver.SetResolver(new AutofacResolver(container));

            TinyMvvm.Forms.TinyMvvm.Initialize();

            MainPage = new AppShell();
        }
    ```
1. Create a new folder called **ViewModels** in the **SampleApp** project.
1. Create two classes in the ViewModels folder, **MainViewModel** and **AboutViewModel**.
1. Add **ViewModelBase** as the base class for MainViewModel and AboutViewModel:
    ```csharp
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
        }
    }
    ```

    ```csharp
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
        }
    }
    ```
1. Edit **MainView.xaml** and **AboutView.xaml** to have **ViewBase** as it's base class. To set BindingContext to the the ViewModel, use the **x:TypeArguments** for the View.
    ```xml
    <mvvm:ViewBase
    xmlns:mvvm="clr-namespace:TinyMvvm.Forms;assembly=TinyMvvm.Forms"
    xmlns:vm="clr-namespace:SampleApp.ViewModels"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="SampleApp.Views.MainView" 
    x:TypeArguments="vm:MainViewModel">

    </mvvm:ViewBase>
    ```
    ```xml
    <mvvm:ViewBase
    xmlns:mvvm="clr-namespace:TinyMvvm.Forms;assembly=TinyMvvm.Forms"
    xmlns:vm="clr-namespace:SampleApp.ViewModels"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="SampleApp.Views.AboutView" 
    x:TypeArguments="vm:AboutViewModel">
 
    </mvvm:ViewBase>
    ```
1. Remove the base class from **MainView.xaml.cs** and **AboutView.xaml.cs**. Because is it a partial class you don't have to specify it both in the XAML-file and in the code-behind file.
1. Create a new View, **DetailsView**, and a new ViewModel, **DetailsViewModel** as in the same style as above.
1. In MainViewModel, add the following below. The override of **Initialize** is running when **BindingContext** for the view has been set. You can also override **OnAppearing** to run code when the view will appear and **OnDisappearing** to run code when the view will disappear. **IsBusy** can be used to bind an **ActivityIndicator** to. For Commands, use **TinyCommand** instead of **Xamarin.Forms.Command** to keep the ViewModel clean from Xamarin.Forms references. To navigate, use the **Navigation** property from **ViewModelBase**. Here you can use the name of a ViewModel as a part of a URL to navigate to a route. With TinyMvvm you can also specify a parameter to pass to the target ViewModel.
    ```csharp
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<string> names;
        public ObservableCollection<string> Names
        {
            get => names;
            set => Set(ref names, value);
        }

        public async override Task Initialize()
        {
            IsBusy = true;            
            await base.Initialize();

            Names = new ObservableCollection<string>(new List<string>()
            {
                "Daniel",
                "Ella",
                "Willner"
            });

            IsBusy = false;
        }

        public override Task OnAppearing()
        {
            return base.OnAppearing();
        }

        public override Task OnDisappearing()
        {
            return base.OnDisappearing();
        }

        private ICommand details;
        private ICommand Details => details ??= new TinyCommand<string>(async(name) =>
        {
            await Navigation.NavigateToAsync($"{nameof(DetailsViewModel)}?name={name}", DateTimeOffset.Now);
        });
    }
    ```
1. To **MainView.xaml** add the following code to show the bind a **CollectionView** to the data in the ViewModel:
    ```xml
     <Grid>
        <ActivityIndicator HorizontalOptions="Center" VerticalOptions="Center" IsRunning="False" IsVisible="{Binding IsBusy}" />
        <CollectionView x:Name="Names" ItemsSource="{Binding Names}" IsVisible="{Binding IsNotBusy}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <ContentView Padding=" 10">
                        <ContentView.GestureRecognizers>
                            <TapGestureRecognizer Command="{Binding Source={x:Reference Names}, Path=BindingContext.Details}" CommandParameter="{Binding}" />
                        </ContentView.GestureRecognizers>
                        <Label Text="{Binding }" />
                    </ContentView>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </Grid>
    ```
1. Go to DetailsViewModel.cs and add the code below to receive the parameters sent to it. **QueryParameters** will be a **Dictionary<string, string>** and contains the query parameters specified in the navigation URL. If you also specified a parameter you can access it via the **NavigationParameter** property.
    ```csharp
    public class DetailsViewModel : ViewModelBase
    {
        public async override Task Initialize()
        {
            await base.Initialize();

            Name = QueryParameters["name"];
            var dateParameter = (DateTimeOffset)NavigationParameter;

            Date = dateParameter.ToString();
        }

        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string date;
        public string Date
        {
            get => date;
            set => Set(ref date, value);
        }
    }
1. In the **DetailsView.cs** add the following code to show the data passed from MainViewModel:
    ```xml
    <mvvm:ViewBase
        xmlns:mvvm="clr-namespace:TinyMvvm.Forms;assembly=TinyMvvm.Forms"
        xmlns:vm="clr-namespace:SampleApp.ViewModels"
        xmlns="http://xamarin.com/schemas/2014/forms"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        x:Class="SampleApp.Views.DetailsView" x:TypeArguments="vm:DetailsViewModel">
        <StackLayout Padding="10">
            <Label Text="{Binding Name}" />
            <Label Text="{Binding Date}" />
        </StackLayout>
    </mvvm:ViewBase>
    ```
1. Navigate to **AboutViewModel.cs** and add the following code to use with a button to navigate to the MainView using the route specified in the **Shell**.
    ```csharp
    public class AboutViewModel : ViewModelBase
    {
        private ICommand home;
        public ICommand Home => home ?? new TinyCommand(async () =>
        {
            await Navigation.NavigateToAsync("//home");
        });
    }
    ```
1. Go to **AboutView.xaml** and create a Button to use with the Command in **AboutViewModel**.
    ```xml
    <mvvm:ViewBase
        xmlns:mvvm="clr-namespace:TinyMvvm.Forms;assembly=TinyMvvm.Forms"
        xmlns:vm="clr-namespace:SampleApp.ViewModels"
        xmlns="http://xamarin.com/schemas/2014/forms"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        x:Class="SampleApp.Views.AboutView" x:TypeArguments="vm:AboutViewModel">
     <Grid>
         <Button Text="Go home!" Command="{Binding Home}" />
     </Grid>
    </mvvm:ViewBase>
    ```

The complete code for this sample can be found here: https://github.com/TinyStuff/TinyMvvm/tree/master/src/Samples
