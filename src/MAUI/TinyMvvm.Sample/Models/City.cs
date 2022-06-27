using System;
namespace TinyMvvm.Sample.Models
{
    public record City
    {
        public string Country { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}
