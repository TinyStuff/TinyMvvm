using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using TinyMvvm.Forms.Sample.iOS.Startup;
using TinyMvvm.Forms.Sample.Startup;
using UIKit;

namespace TinyMvvm.Forms.Sample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Bootstrapper.Platform = new IosBootstrapper();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
