using TinyMvvm.Sample.ViewModels;

namespace TinyMvvm.Sample.Views;

public partial class ListView
{
	public ListView(ListViewModel listViewModel)
	{
		InitializeComponent();

		BindingContext = listViewModel;
	}
}
