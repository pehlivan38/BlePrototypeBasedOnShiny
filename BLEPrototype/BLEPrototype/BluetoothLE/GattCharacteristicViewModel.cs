using BLEPrototype.ViewModels;
using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLEPrototype.BluetoothLE
{
    public class GattCharacteristicViewModel : BLEViewModel
    {

        public IGattCharacteristic Characteristic { get; }


        public string Value { get; private set; }
        public bool IsNotifying { get; private set; }
        public bool IsValueAvailable { get; private set; }
        public DateTime LastValue { get; private set; }
        public Guid Uuid => this.Characteristic.Uuid;
        public Guid ServiceUuid => this.Characteristic.Service.Uuid;
        public string Description => this.Characteristic.Description;
        public string Properties => this.Characteristic.Properties.ToString();


        public GattCharacteristicViewModel(IGattCharacteristic characteristic)
        {
            this.Characteristic = characteristic;
        }
    }
}
