using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TinyMvvm.Autofac;
using TinyMvvm.IoC;
using TinyMvvm.WPF.Sample.ViewModels;

namespace TinyMvvm.WPF.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           

            var builder = new ContainerBuilder();

            builder.RegisterType<MainViewModel>();

            var container = builder.Build();

            Resolver.SetResolver(new AutofacResolver(container));

            InitializeComponent();
        }
    }
}
