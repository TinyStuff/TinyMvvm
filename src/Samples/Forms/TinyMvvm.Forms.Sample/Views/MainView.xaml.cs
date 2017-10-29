using System;
using System.Collections.Generic;
using TinyMvvm.Forms.Sample.ViewModels;
using Xamarin.Forms;

namespace TinyMvvm.Forms.Sample.Views
{
    public partial class MainView : ViewBase<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}
