using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;
using FFImageLoading.Forms.Touch;
using ImageCircle.Forms.Plugin.iOS;
using ButtonCircle.FormsPlugin.iOS;

namespace IsDatSteve.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            ButtonCircleRenderer.Init();
            CachedImageRenderer.Init();
            ImageCircleRenderer.Init();
            LoadApplication(new App(new iOSInitializer()));
            FormsPlugin.Iconize.iOS.IconControls.Init();
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
