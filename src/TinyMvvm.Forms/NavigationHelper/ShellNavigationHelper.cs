using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TinyMvvm.Forms
{
    public class ShellNavigationHelper : FormsNavigationHelper
    {
        private Dictionary<string, string> queries = new Dictionary<string, string>();
        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        public ShellNavigationHelper()
        {
            NavigationHelper.Current = this;
        }

        public void RegisterRoute(string route, Type type)
        {
            try
            {

                Routing.RegisterRoute(route, type);
            }
            catch (ArgumentException)
            {
                //Catch to avoid app crash if route already registered
            }
        }

        internal Dictionary<string, string> GetQueryParameters(string tinyId)
        {
            var query = queries[tinyId];

            var values = query.Split('&');

            var parameters = new Dictionary<string, string>();

            foreach (var val in values)
            {
                var split = val.Split('=');

                parameters.Add(split.First(), split.Last());
            }

            parameters.Remove(tinyId);

            return parameters;
        }

        internal object? GetParameter(string tinyId)
        {
            if(parameters.ContainsKey(tinyId))
            {
                var parameter = parameters[tinyId];

                parameters.Remove(tinyId);

                return parameter;
            }

            return null;
        }

        public async override Task NavigateToAsync(string key)
        {

                if (Views.ContainsKey(key.ToLower()))
                {
                    await base.NavigateToAsync(key);
                    return;
                }

                if (key.Contains("?"))
                {
                    var route = key.Split('?');

                    var tinyId = Guid.NewGuid().ToString();
                    key = $"{key}&tinyid={tinyId}";

                    queries.Add(tinyId, route.Last());
                }

                await Shell.Current.GoToAsync(key);
           
        }

        public async override Task NavigateToAsync(string key, object parameter)
        {
            try
            {
                if (Views.ContainsKey(key.ToLower()))
                {
                    await base.NavigateToAsync(key, parameter);
                    return;
                }

                var tinyId = Guid.NewGuid().ToString();

                if (key.Contains("?"))
                {
                    var route = key.Split('?');
                    
                    key = $"{key}&tinyid={tinyId}";

                    queries.Add(tinyId, route.Last());
                }
                else
                {
                    key = $"{key}?tinyid={tinyId}";
                }

                parameters.Add(tinyId, parameter);

                await Shell.Current.GoToAsync(key);
            }
            catch (Exception)
            {
                await base.NavigateToAsync(key);
            }
        }

        protected override void InternalRegisterView(Type type, string key)
        {
            base.InternalRegisterView(type, key);

            if (type.GenericTypeArguments.Length > 0)
            {
                RegisterRoute(type.GenericTypeArguments[0].Name, type);
            }

            if (type.BaseType.GenericTypeArguments.Length > 0)
            {
                RegisterRoute(type.BaseType.GenericTypeArguments[0].Name, type);
            }

            RegisterRoute(key, type);
        }
    }
}
