using TinyMvvm.Sample.ViewModels;

namespace TinyMvvm.Sample.Views;

public partial class MainView
{
	public MainView(MainViewModel mainViewModel)
	{
		InitializeComponent();

		BindingContext = mainViewModel;
	}
}
