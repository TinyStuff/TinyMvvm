using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TinyMvvm
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        Task Initialize();

        Task OnAppearing();

        Task OnFirstAppear();

        Task OnDisappearing();

        object? NavigationParameter { get; set; }
        Dictionary<string, string>? QueryParameters { get; set; }
    }
}
