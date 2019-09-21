using BLEPrototype.ViewModels;
using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void Select()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(1))
            {
                DoRead();
            }

            s.Stop();
            return;
        }

        async void DoRead()
        {
            //var utf8 = await this.dialogs.Confirm(
            //    "Display Value as UTF8 or HEX?",
            //    okText: "UTF8",
            //    cancelText: "HEX"
            //);
            this.Characteristic
                .Read()
                //.Timeout(TimeSpan.FromSeconds(2))
                .Subscribe(
                    x => this.SetReadValue(x, true),
                    ex => Console.WriteLine("ERROR: " + ex.ToString())
                );

        }

        void SetReadValue(CharacteristicGattResult result, bool fromUtf8) => Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
        {
            this.IsValueAvailable = true;
            this.LastValue = DateTime.Now;

            if (result.Data == null)
                this.Value = "EMPTY";

            else
                this.Value = result.Data[0].ToString();

            RaisePropertyChanged(nameof(IsValueAvailable));
            RaisePropertyChanged(nameof(LastValue));
            RaisePropertyChanged(nameof(Value));

            //fromUtf8
            //    ? Encoding.UTF8.GetString(result.Data, 0, result.Data.Length)
            //    : BitConverter.ToString(result.Data);
        });
    }
}
