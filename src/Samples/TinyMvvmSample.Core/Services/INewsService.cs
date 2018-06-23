using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TinyMvvmSample.Core.Models;
namespace TinyMvvmSample.Core.Services
{
    public interface INewsService
    {
        Task<List<NewsItem>> Get();
    }
}
