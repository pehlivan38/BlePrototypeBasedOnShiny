using BLEPrototype.ViewModels;
using Prism.Commands;
using Prism.Mvvm;
using Shiny;
using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace BLEPrototype.BluetoothLE
{
    public class AdapterViewModel : BLEViewModel
    {

        public string HelloString => "This page is to manage your BLE connections";

        readonly ICentralManager _centralManager;
        IDisposable _scan;

        public DelegateCommand OpenSettingsCommand { get; }
        public DelegateCommand ScanToggleCommand { get;  }

        public AdapterViewModel(ICentralManager central)
        {
            _centralManager = central;
            OpenSettingsCommand = new DelegateCommand(OpenSettings);
            ScanToggleCommand = new DelegateCommand(ScanToggle);
        }

        #region peripheral operations
        public ObservableList<PeripheralItemViewModel> Peripherals { get; } = new ObservableList<PeripheralItemViewModel>();

        #endregion

        private void OpenSettings()
        {
            var settings = _centralManager as ICanOpenAdapterSettings;
            try
            {
                settings.OpenSettings();
            } // I keep getting 
            catch(InvalidOperationException e)
            {
                // I keep getting WinRT information: A method was called at an unexpected time. Shiny.BluetoothLE.dll
                // exception - and I do not have any idea why - but it is ok to continue.
                Console.WriteLine(e.Message);
            }
        }

        public bool IsScanning { get; private set; }
        private void ScanToggle()
        {
            if (this.IsScanning)
            {
                this.IsScanning = false;
                this._scan?.Dispose();
            }
            else
            {
                this.Peripherals.Clear();

                this._scan = _centralManager
                    .Scan()
                    .Buffer(TimeSpan.FromSeconds(1))
                    .Synchronize()
                    .Subscribe(
                        results =>
                        {
                            var list = new List<PeripheralItemViewModel>();
                            foreach (var result in results)
                            {
                                var peripheral = this.Peripherals.FirstOrDefault(x => x.Equals(result.Peripheral));
                                if (peripheral == null)
                                    peripheral = list.FirstOrDefault(x => x.Equals(result.Peripheral));

                                if (peripheral != null)
                                    peripheral.Update(result);
                                else
                                {
                                    peripheral = new PeripheralItemViewModel(result.Peripheral);
                                    peripheral.Update(result);
                                    list.Add(peripheral);
                                }
                            }
                            if (list.Any())
                                this.Peripherals.AddRange(list);

                            RaisePropertyChanged(nameof(Peripherals));
                        },
                        ex => Console.WriteLine("ERROR: " + ex.ToString())
                    );

                this.IsScanning = true;
                
            }
            RaisePropertyChanged(nameof(IsScanning));
        }

    }
}
