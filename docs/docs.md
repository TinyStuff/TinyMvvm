# TinyMvvm (for Xamarin Forms)

**<div style="color: red;">In progress - not done yet</div>**

TinyMvvm is yet another small MVVM framework designed specifically with Xamarin Forms in mind. There is a TinyMvvm-version for WPF in progress as well, but this documentation does not cover that weird version.

This documentation does not cover the basics of MVVM and assumes that you are already familiar with the concept of MVVM.

## The overall structure

### ViewBase&lt;T&gt;

Features (or drawbacks) of the ViewBase

* It strongly types the ViewModel as any class implementing INotifyPropertyChanged
* It creates the ViewModel for you if you inherit the ViewModel from ViewModelBase

The view should inherit from `ViewBase<T>` where `T` is the ViewModel. The ViewModel can be any class that implements `INotifyPropertyChanged`. 

`ViewBase<T>` itself inherits from `Xamarin.Forms.ContentPage` and can be treated by Xamarin Forms as any page.

If you decide to use `ViewModelBase` as the base class for your view model and at the same time have the IoC resolver enabled, the the view will automatically create the view model for you when the view is created. Hence no need to inject the view model in the constructor and assign it manually. Feature or not, you decide.

### ViewModelBase

TinyMvvm also defines a base class for the view model called `ViewModelBase`.

Features (or drawbacks) of the ViewModelBase

* Wraps navigation for you through the INavigation interface (Implemented in TinyNavigation)
* Wraps a tiny pub/sub framework (Implemented in TinyPubSub)
* Implements INotifyPropertyChanged for you
* Propagates life cycle events to the view (Initialize, OnAppearing, OnDisapparing)

Navigation is made really easy. In the xaml, simply bind a command to `NavigateTo` and pass the view name as the parameter:

```xml
<Button Text="About" Command="{Binding NavigateTo}" CommandParameter="AboutView" />
``` 

### IoC

TODO

### TinyCommand

ToOO

