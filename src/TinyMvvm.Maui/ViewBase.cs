namespace TinyMvvm.Maui;

public abstract class ViewBase : ContentPage
{
    internal SemaphoreSlim ReadLock { get; private set; } = new SemaphoreSlim(1, 1);
    internal protected bool isShellView;

    protected async override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is IViewModelBase viewModel)
        {  
            try
            {
                if (!viewModel.IsInitialized)
                {
                    await ReadLock.WaitAsync();
                    await viewModel.Initialize();

                    viewModel.IsInitialized = true;
                }
            }
            finally
            {
                if (!isShellView)
                {
                    ReadLock.Release();
                }
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IViewModelBase viewModel)
        {
            onAppearing(){
                await ReadLock.WaitAsync();

                if (!viewModel.IsInitialized)
                {
                    await viewModel.Initialize();
                    viewModel.IsInitialized = true;
                }

                await viewModel.OnAppearing();

                ReadLock.Release();
            };

            if(MainThread)
               
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IViewModelBase viewModel)
        {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ReadLock.WaitAsync();                 

                    await viewModel.OnAppearing();

                    ReadLock.Release();
                });
        }
    }

    internal virtual void CreateViewModel()
    {

    }
}