using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BLEPrototype.Startup;
using Shiny;

namespace BLEPrototype.Droid
{
    [Application]
    public class MainApplication : ShinyAndroidApplication<BLEStartup>
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }
    }
}