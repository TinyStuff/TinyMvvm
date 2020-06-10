using System;
using System.Threading.Tasks;
using TinyMvvm;

namespace SampleApp.ViewModels
{
    public class DetailsViewModel : ViewModelBase
    {
        public async override Task Initialize()
        {
            await base.Initialize();

            Name = QueryParameters["name"];
            var dateParameter = (DateTimeOffset)NavigationParameter;

            Date = dateParameter.ToString();
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
    }
}
