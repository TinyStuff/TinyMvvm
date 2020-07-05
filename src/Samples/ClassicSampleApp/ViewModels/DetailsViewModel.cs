using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;

namespace ClassicSampleApp.ViewModels
{
    public class DetailsViewModel : ViewModelBase
    {
        public async override Task Initialize()
        {
            await base.Initialize();

            Name = (string)NavigationParameter;

        }

        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string date;
        public string Date
        {
            get => date;
            set => Set(ref date, value);
        }

        public ICommand About => new TinyCommand(() =>
        {
            Navigation.SetRootView("AboutView");
        });
    }
}
