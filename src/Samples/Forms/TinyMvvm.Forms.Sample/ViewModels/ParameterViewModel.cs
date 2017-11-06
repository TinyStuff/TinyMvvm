using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMvvm.Forms.Sample.ViewModels
{
    public class ParameterViewModel : ViewModelBase
    {
        public string HelloText { get; set; }
        public async override Task Initialize()
        {
            await base.Initialize();

            HelloText = NavigationParameter.ToString();

            RaisePropertyChanged("HelloText");
        }
    }
}
