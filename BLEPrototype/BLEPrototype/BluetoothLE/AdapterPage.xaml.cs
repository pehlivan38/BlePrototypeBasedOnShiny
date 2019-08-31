using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLEPrototype.BluetoothLE
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdapterPage : ContentPage
    {
		public AdapterPage ()
		{
			InitializeComponent ();
		}
	}
}