using Shiny;
using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLEPrototype.Delegates
{
    public class BleDelegates : IBleAdapterDelegate, IBlePeripheralDelegate
    {
        public void OnBleAdapterStateChanged(AccessState state)
        {
            ;
        }

        public void OnConnected(IPeripheral peripheral)
        {
            ;
        }
    }
}
