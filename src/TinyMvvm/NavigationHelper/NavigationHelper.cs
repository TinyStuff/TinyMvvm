using System;
using System.Collections.Generic;
using System.Text;

namespace TinyMvvm
{
    public class NavigationHelper
    {
        public static INavigationHelper Current { get; set; } = null!;
    }
}
