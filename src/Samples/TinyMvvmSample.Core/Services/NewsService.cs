using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyMvvmSample.Core.Models;

namespace TinyMvvmSample.Core.Services
{
    public class NewsService : INewsService
    {
        public async Task<List<NewsItem>> Get()
        {
            return new List<NewsItem>()
            {
                new NewsItem(){Title = "1.0 of TinyMvvm release!", Text = "The people behind TinyStuff have release 1.0 of TinyMvvm"},
                new NewsItem(){Title = "TinyPubSub", Text = "Read more about TinyPubSub"},
                new NewsItem(){Title = "TinyCache", Text = "New version of TinyCache"}
            };
        }
    }
}
