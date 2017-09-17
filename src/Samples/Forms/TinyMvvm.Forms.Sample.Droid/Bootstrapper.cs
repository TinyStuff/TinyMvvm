using System;
using Autofac;

namespace TinyMvvm.Forms.Sample.Droid
{
	public static class Bootstrapper
	{
		public static void Initialize()
		{
			var builder = new ContainerBuilder();

			// Common stuff
			TinyMvvm.Forms.Sample.Bootstrapper.Initialize(builder);

			// iOS specific stuff

			// Build and set
			var container = builder.Build();
			var resolver = new TinyMvvm.Autofac.AutofacResolver(container);
			TinyMvvm.IoC.Resolver.SetResolver(resolver);
		}
	}
}
