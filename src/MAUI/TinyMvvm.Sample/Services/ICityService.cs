using System;
using TinyMvvm.Sample.Models;

namespace TinyMvvm.Sample.Services
{
	public interface ICityService
	{
        Task<List<City>> Search(string text);
        Task<City> Get(string city);
        Task<List<City>> GetAll();
    }
}

