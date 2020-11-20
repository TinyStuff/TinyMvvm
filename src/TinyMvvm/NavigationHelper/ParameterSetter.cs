using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TinyMvvm
{
    public static class ParameterSetter
    {
        public static bool CanSet(Type type)
        {
            try
            {
                var property = type.GetRuntimeProperty("NavigationParameter");

                if (property != null)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static void Set(object view, object parameter)
        {
            try
            {
                var property = view.GetType().GetRuntimeProperty("NavigationParameter");

                if(property != null)
                {
                    property.SetValue(view, parameter);
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
