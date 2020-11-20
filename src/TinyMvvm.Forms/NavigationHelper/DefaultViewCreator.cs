using System;
using System.Collections.Generic;
using System.Text;
using TinyMvvm;
using Xamarin.Forms;

namespace TinyMvvm.Forms
{
    public class DefaultViewCreator : IViewCreator<Page>
    {
        public Page Create(Type type)
        {         
            return (Page)Activator.CreateInstance(type);
        }

        public Page Create(Type type, object parameter)
        {
            if (ParameterSetter.CanSet(type))
            {
                var page = Create(type);

                ParameterSetter.Set(page, parameter);

                return page;
            }
            else
            {
                return (Page)Activator.CreateInstance(type, parameter);
            }
        }
    }
}
