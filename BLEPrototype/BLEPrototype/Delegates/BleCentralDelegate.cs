using Shiny;
using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLEPrototype.Delegates
{
    public class BleCentralDelegate : IBleCentralDelegate
    {
        public Task OnAdapterStateChanged(AccessState state)
        {
            return Task.CompletedTask;
        }

        public Task OnConnected(IPeripheral peripheral)
        {
            return Task.CompletedTask;
        }
    }
}
