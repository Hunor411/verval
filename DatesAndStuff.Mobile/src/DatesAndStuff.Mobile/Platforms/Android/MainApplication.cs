using Android.App;
using Android.Runtime;

namespace DatesAndStuff.Mobile
{
    using System;
    using Microsoft.Maui;
    using Microsoft.Maui.Hosting;

    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
