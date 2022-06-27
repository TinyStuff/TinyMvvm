using System.Reflection;
using TinyMvvm;
using TinyMvvm.Sample.Services;
using TinyMvvm.Sample.ViewModels;
using TinyMvvm.Sample.Views;
using ListView = TinyMvvm.Sample.Views.ListView;

namespace TinyMvvm.Sample;


public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var assembly = Assembly.GetExecutingAssembly();

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseTinyMvvm();

		builder.Services.AddSingleton<ICityService, CityService>();

		builder.Services.AddTransient<MainView>();
        builder.Services.AddTransient<ListView>();
        builder.Services.AddTransient<DetailsView>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<ListViewModel>();
        builder.Services.AddTransient<DetailsViewModel>();

		Routing.RegisterRoute(nameof(DetailsViewModel), typeof(DetailsView));

        return builder.Build();
	}
}

