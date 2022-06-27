using System;
using System.Windows.Input;
using TinyMvvm;

namespace ClassicSampleApp.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private ICommand home;
        public ICommand Home => home ?? new TinyCommand(async () =>
        {
            await Navigation.NavigateToAsync("MainView");
        });

    }
}
