using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TinyNavigationHelper.Forms
{
    public class ShellNavigationHelper : FormsNavigationHelper
    {
        private List<string> routes = new List<string>();
        private Dictionary<string, string> queries = new Dictionary<string, string>();

        public void RegisterRoute(string route, Type type)
        {
            Routing.RegisterRoute(route, type);

            routes.Add(route);
        }

        internal Dictionary<string, string> GetQueryParameters(string tinyId)
        {
            var values = queries[tinyId].Split('&');

            var parameters = new Dictionary<string, string>();

            foreach(var val in values)
            {
                var split = val.Split('=');

                parameters.Add(split.First(), split.Last());
            }

            return parameters;
        }

        public async override Task NavigateToAsync(string key)
        {
            try {

               var route = key.TrimStart('/').Split('?');

                if (routes.Contains(route.First()))
                {
                    if (key.Contains("?"))
                    {
                        var tinyId = Guid.NewGuid().ToString();
                        key = $"{key}&tinyid={tinyId}";

                        queries.Add(tinyId, route.Last());
                    }
                }

                await Shell.Current.GoToAsync(key);
            }
            catch (Exception ex)
            {
                await base.NavigateToAsync(key);
            }
  
                
     
        }
    }
}
