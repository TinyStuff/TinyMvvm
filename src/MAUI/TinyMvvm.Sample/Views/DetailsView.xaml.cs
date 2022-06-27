using TinyMvvm.Sample.ViewModels;

namespace TinyMvvm.Sample.Views;

public partial class DetailsView
{
	public DetailsView(DetailsViewModel detailsViewModel)
	{
		InitializeComponent();

		BindingContext = detailsViewModel;

		
	}
}
