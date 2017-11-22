# TinyMvvm (for Xamarin Forms)

**<div style="color: red;">In progress - not done yet</div>**

TinyMvvm is yet another small MVVM framework designed specifically with Xamarin Forms in mind. There is a TinyMvvm-version for WPF in progress as well, but this documentation does not cover that weird version.

This documentation does not cover the basics of MVVM and assumes that you are already familiar with the concept of MVVM.

## What do we do?

* Supply a base class for views and viewmodels
* Implementation of `INotifyPropertyChanged`
* Wraps TinyNavigationService for easy as pie navigation

## How to install
Install the TinyMvvm.Forms package from NuGet, https://www.nuget.org/packages/TinyMvvm.Forms/

```
Install-Package TinyMvvm.Forms 
```
If you want to separet the ViewModels in a separte project that dosen't have references to Xamarin.Forms, just install the TinyMvvm packatge on that project.

```
Install-Package TinyMvvm 
```


## The overall structure

### ViewBase&lt;T&gt;

Features (or drawbacks) of the ViewBase

* It strongly types the ViewModel as any class implementing INotifyPropertyChanged
* It creates the ViewModel for you if you inherit the ViewModel from ViewModelBase

The view should inherit from `ViewBase<T>` where `T` is the ViewModel. The ViewModel can be any class that implements `INotifyPropertyChanged`. 

`ViewBase<T>` itself inherits from `Xamarin.Forms.ContentPage` and can be treated by Xamarin Forms as any page.

If you decide to use `ViewModelBase` as the base class for your view model and at the same time have the IoC resolver enabled, the the view will automatically create the view model for you when the view is created. Hence no need to inject the view model in the constructor and assign it manually. Feature or not, you decide.

An example of a typical page in TinyMvvm would look like this xaml copied from the sample app.

```xml
<?xml version="1.0" encoding="UTF-8"?>
<tinymvvm:ViewBase x:TypeArguments="viewmodels:MainViewModel" 
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:viewmodels="clr-namespace:TinyMvvm.Forms.Sample.ViewModels;assembly=TinyMvvm.Forms.Samples"
    xmlns:tinymvvm="clr-namespace:TinyMvvm.Forms;assembly=TinyMvvm.Forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    x:Class="TinyMvvm.Forms.Sample.Views.MainView">
    
	<ContentPage.Content>
        <StackLayout Margin="10,40">
            <Label Text="Hello World" />
            <Button Text="About" Command="{Binding NavigateTo}" CommandParameter="AboutView" />
        </StackLayout>
    </ContentPage.Content>
    
</tinymvvm:ViewBase>
```

What you need to do is:

* Define two namespaces (viewmodels and tinymvvm)
* Change the view type to `tinymvvm:ViewBase`
* Add a type argument pointing to your view model

The only thing that changes in the code behind is the base class.

```csharp
public partial class MainView : TinyMvvm.Forms.ViewBase<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}
```

### ViewModelBase

TinyMvvm also defines a base class for the view model called `ViewModelBase`.

Features (or drawbacks) of the ViewModelBase

* Wraps navigation for you through the INavigation interface (Implemented in TinyNavigation)
* Wraps a tiny pub/sub framework (Implemented in TinyPubSub)
* Implements INotifyPropertyChanged for you
* Propagates life cycle events to the view (Initialize, OnAppearing, OnDisapparing)

### Navigation
TinyMvvm using the TinyNavigationHelper for navigation, for more detailed information, please read the documentation for TinyNavigationHelper, https://github.com/dhindrik/TinyNavigationHelper/blob/master/README.md.

#### Navigation in View (from Xaml)

Navigation is made really easy. In the xaml, simply bind a command to `NavigateTo` and pass the view name as the parameter:

```xml
<Button Text="About" Command="{Binding NavigateTo}" CommandParameter="AboutView" />
```

or open a view as a modal view:

```xml
<Button Text="About" Command="{Binding OpenModal}" CommandParameter="AboutView" />
```

#### Navigation from ViewModel (you know, from code)

```csharp
await Navigation.NavigateToAsync("AboutView");
```

or open as modal

```csharp
await Navigation.OpenModalAsync("AboutView");
```

#### Messaging

TinyMvvm has no messaging system built-in, we recommend you to take a look at TinyPubSub if you need messaging in your app.

Check out [TinyPubSub](https://github.com/johankson/TinyPubSub) for more detailed information about it.

### IoC

TODO

### TinyCommand

ToOO

