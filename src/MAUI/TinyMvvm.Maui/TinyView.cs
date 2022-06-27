namespace TinyMvvm;

public abstract class TinyView : ContentPage
{
    internal SemaphoreSlim ReadLock { get; private set; } = new SemaphoreSlim(1, 1);

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is ITinyViewModel viewModel)
        {
            try
            {
                if (!viewModel.IsInitialized)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await ReadLock.WaitAsync();
                        await viewModel.Initialize();

                        viewModel.IsInitialized = true;
                    });
                }
            }
            finally
            {
                ReadLock.Release();
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ITinyViewModel viewModel)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await ReadLock.WaitAsync();

                if (!viewModel.IsInitialized)
                {
                    await viewModel.Initialize();
                    viewModel.IsInitialized = true;
                }

                await viewModel.OnAppearing();

                ReadLock.Release();
            });

        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is ITinyViewModel viewModel)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await ReadLock.WaitAsync();

                await viewModel.OnDisappearing();

                ReadLock.Release();
            });
        }
    }
}