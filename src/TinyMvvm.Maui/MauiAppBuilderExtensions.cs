using System;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TinyMvvm.Maui
{
    public static class MauiAppBuilderExtensions
    {
        /// <summary>
        /// Setup TinyMvvm when using Shell. Types for Pages and ViewModels will be registered into the Dependency Injection container.
        /// </summary>
        /// <param name="viewAssembly">The assembly where the views are located. If not specify it will use the executing assembly.</param>
        /// <param name="viewModelAssembly">The assembly where the view models are located. If not specify it will use the executing assembly.</param>
        /// <returns>The Maui App Builder</returns>
        public static MauiAppBuilder UseTinyMvvm(this MauiAppBuilder builder, Assembly? viewAssembly = null, Assembly? viewModelAssembly = null)
        {
            if (viewAssembly == null)
            {
                viewAssembly = Assembly.GetEntryAssembly();
            }

            if (viewModelAssembly == null)
            {
                viewModelAssembly = Assembly.GetEntryAssembly();
            }

            var navigationHelper = new ShellNavigationHelper();

            navigationHelper.InitViewModelNavigation(viewModelAssembly);

            builder.Services.AddSingleton<INavigationHelper>(navigationHelper);

            RegisterAssemblies(builder, viewAssembly, viewModelAssembly);

            return builder;
        }
        /// <summary>
        /// Setup TinyMvvm when using Shell or the classic navigation with NavigationPage. Types for Pages and ViewModels will be registered into the Dependency Injection container.
        /// </summary>
        /// /// <param name="useClassicNavigation">Set to true to use navigation with NavigationPage</param>
        /// <param name="viewAssembly">The assembly where the views are located. If not specify it will use the executing assembly.</param>
        /// <param name="viewModelAssembly">The assembly where the view models are located. If not specify it will use the executing assembly.</param>
        /// <returns>The Maui App Builder</returns>
        /// <param name="builder"></param>
        public static MauiAppBuilder UseTinyMvvm(this MauiAppBuilder builder, bool useClassicNavigation, Assembly? viewAssembly = null, Assembly? viewModelAssembly = null)
        {
            if(!useClassicNavigation)
            {
                return UseTinyMvvm(builder, viewAssembly, viewModelAssembly);
            }

            if (viewAssembly == null)
            {
                viewAssembly = Assembly.GetExecutingAssembly();
            }

            if (viewModelAssembly == null)
            {
                viewModelAssembly = Assembly.GetExecutingAssembly();
            }

            var navigationHelper = new ClassicNavigationHelper();
            navigationHelper.RegisterViewsInAssembly(viewAssembly);

            builder.Services.AddSingleton<INavigationHelper>(navigationHelper);

            RegisterAssemblies(builder, viewAssembly, viewModelAssembly);

            return builder;
        }

        private static void RegisterAssemblies(MauiAppBuilder builder, Assembly viewAssembly, Assembly viewModelAssembly)
        {
            var pageTypes = viewAssembly.DefinedTypes.Select(x => x.AsType()).Where(x => x.IsSubclassOf(typeof(ViewBase)) && !x.IsAbstract);

            foreach (var type in pageTypes)
            {
                builder.Services.AddTransient(type);
            }

            var typesInAssembly = viewModelAssembly.DefinedTypes.Select(x => x.AsType()).Where(x => x.IsSubclassOf(typeof(ViewModelBase)) && !x.IsAbstract);

            foreach (var type in typesInAssembly)
            {
                builder.Services.AddTransient(type);
            }
        }
    }
}

