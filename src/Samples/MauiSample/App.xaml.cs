using System.Reflection;
using Autofac;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using TinyMvvm.Autofac;

namespace MauiSample;

public partial class App
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
