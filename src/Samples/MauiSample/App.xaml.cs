using System.Reflection;
using Autofac;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using TinyMvvm.Autofac;
using Application = Microsoft.Maui.Controls.Application;

namespace MauiSample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
