TinyMvvm for .NET MAUI have moved here, https://github.com/dhindrik/TinyMvvm. 

This repo is home for the Xamarin.Forms code.

## Build status
<img src="https://io2gamelabs.visualstudio.com/_apis/public/build/definitions/be16d002-5786-41a1-bf3b-3e13d5e80aa0/8/badge" alt="Build Status" />

## 4.0 preview - .NET MAUI
Watch this video for more info about TinyMvvm and .NET MAUI. Docs will be published soon.

https://youtu.be/XnBmvOu3MO4

## 3.0 Breaking changes
* **TinyNavigationHelper** is merged into TinyMvvm and the namespaced is changed to **TinyMvvm**.
* To use ViewModel navigation with shell you have to call **InitViewModelNavigation**.

```csharp
navigationHelper.InitViewModelNavigation(viewModelAssembly);
```

## What is TinyMvvm?
TinyMvvm is a MVVM library that is built for Xamarin.Forms but is not limited to Xamarin.Forms, it created in a way that it will be easy to extend to other platforms.

TinyMvvm main features:
* NavigationHelper that supports both Shell and the classic NavigationService.
* Passing objects as parameters, both when using Shell and classic navigation.
* ViewModel navigation with Shell.
* Add BindingContext in View, but still be able to use DI in VIewModels.
* Lifecycle events in ViewModels.
* INotiftyPropertyChanged implementation.

## Get started

To get started with TinyMvvm the recommendation is to following this, <a href="https://github.com/dhindrik/TinyMvvm/blob/master/docs/GetStarted.md">Get Started Tutorial</a>.

You can also read the following document about the main concepts of TinyMvvm,
<a href="https://github.com/dhindrik/TinyMvvm/blob/master/docs/docs.md">TinyMvvm Concepts</a>

Watch Daniel Hindrikes talking about TinyMvvm on YouTube with this recording from Xam Expert Day 2020:
[![Watch on YouTube](https://img.youtube.com/vi/rS-cnU86870/0.jpg)](https://www.youtube.com/watch?v=rS-cnU86870)

## Contribute
You are very welcome to contribute to TinyMvvm. If you want to add a new feature we would like if you create an issue first so we can discuss the the feature before you spend time to implement it.
