namespace TinyMvvm
{
    public static class MauiAppBuilderExtensions
    {
        /// <summary>
        /// Setup TinyMvvm when using Shell. Types for Pages and ViewModels will be registered into the Dependency Injection container.
        /// </summary>
        /// <param name="viewAssembly">The assembly where the views are located. If not specify it will use the executing assembly.</param>
        /// <param name="viewModelAssembly">The assembly where the view models are located. If not specify it will use the executing assembly.</param>
        /// <returns>The Maui App Builder</returns>
        public static MauiAppBuilder UseTinyMvvm(this MauiAppBuilder builder)
        {
            var navigation = new TinyNavigation();


            builder.Services.AddSingleton<INavigation>(navigation);
            

            return builder;
        } 
    }
}

