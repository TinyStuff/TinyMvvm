using System;
using System.Windows.Input;

namespace TinyMvvm.Forms.Sample.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public override System.Threading.Tasks.Task Initialize()
        {
            return base.Initialize();
        }

        public ICommand WithParameter
        {
            get
            {
                return new TinyCommand(async() =>
                {
                    await Navigation.NavigateToAsync("ParameterView", "Hi Hello");
                });
            }
        }
    }
}
