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
using Xamarin.Forms;
using Prism.Navigation;

namespace BLEPrototype.BluetoothLE
{
    public class AdapterViewModel : BLEViewModel
    {

        public string HelloString => "This page is to manage your BLE connections";

        readonly INavigationService _navigator;
        readonly ICentralManager _centralManager;
        IDisposable _scan;

        public DelegateCommand OpenSettingsCommand { get; }
        public DelegateCommand ScanToggleCommand { get;  }

        public DelegateCommand<PeripheralItemViewModel> SelectPeripheralCommand { get;  }

        public AdapterViewModel(ICentralManager central, INavigationService navigator)
        {
            _centralManager = central;
            _navigator = navigator;
            OpenSettingsCommand = new DelegateCommand(OpenSettings);
            ScanToggleCommand = new DelegateCommand(ScanToggle);

            SelectPeripheralCommand = new DelegateCommand<PeripheralItemViewModel>(SelectPeripheral);
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
                            Device.BeginInvokeOnMainThread(() =>
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
                            });
                            
                        },
                        ex => Console.WriteLine("ERROR: " + ex.ToString())
                    );

                this.IsScanning = true;
                
            }
            RaisePropertyChanged(nameof(Peripherals));
            RaisePropertyChanged(nameof(IsScanning));
        }

        private async void SelectPeripheral(PeripheralItemViewModel item)
        {
            var p = new NavigationParameters();
            p.Add("Peripheral", item.Peripheral);

            var result = await _navigator.NavigateAsync("Peripheral", p);
            if (!result.Success)
                Console.WriteLine("[NAV FAIL] " + result.Exception);
        }
    }
}
