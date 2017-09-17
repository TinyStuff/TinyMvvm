using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace TinyMvvm.Forms.Sample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            var formsApp = new App();
			Bootstrapper.Initialize(formsApp);
            LoadApplication(formsApp);

            return base.FinishedLaunching(app, options);
        }
    }
}
