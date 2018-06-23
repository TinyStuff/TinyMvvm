using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;
using TinyMvvmSample.Core.Services;
using System.Collections.ObjectModel;
using TinyMvvmSample.Core.Models;

namespace TinyMvvmSample.Core.ViewModels
{
    public class NewsViewModel : ViewModel
    {
        private INewsService newsService;

        public NewsViewModel(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public ObservableCollection<NewsItem> Items { get; set; }

        public async override Task Initialize()
        {
            IsBusy = true;
            await base.Initialize();

            var items = await newsService.Get();

            Items = new ObservableCollection<NewsItem>(items);

            RaisePropertyChanged(nameof(Items));

            IsBusy = false;
        }

        public ICommand Refresh => new TinyCommand(async() =>
        {
            IsBusy = true;

            await Task.Delay(500);

            Items.Insert(0, new NewsItem() { Title = "New item", Text = "The list has been refreshed" });

            IsBusy = false;
        });
    }
}
