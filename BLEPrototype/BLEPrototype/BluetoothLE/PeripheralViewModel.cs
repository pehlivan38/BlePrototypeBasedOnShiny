using BLEPrototype.Models;
using BLEPrototype.ViewModels;
using Prism.Commands;
using Prism.Navigation;
using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BLEPrototype.BluetoothLE
{
    public class PeripheralViewModel : BLEViewModel
    {
        readonly ICentralManager _centralManager;
        IPeripheral peripheral;

        public string Name { get; private set; }
        public Guid Uuid { get; private set; }
        public string PairingText { get; private set; }
        public ObservableCollection<Group<GattCharacteristicViewModel>> GattCharacteristics { get; } = new ObservableCollection<Group<GattCharacteristicViewModel>>();

        public string ConnectText { get; private set; } = "Connect";
        public int Rssi { get; private set; }

        public DelegateCommand PairToDeviceCommand { get; }
        public DelegateCommand ConnectionToggleCommand { get; }

        public DelegateCommand<GattCharacteristicViewModel> SelectCharacteristicsCommand { get; }

        public PeripheralViewModel(ICentralManager centralManager)
        {
            _centralManager = centralManager;
            PairToDeviceCommand = new DelegateCommand(PairToDevice);
            ConnectionToggleCommand = new DelegateCommand(ConnectionToggle);

            SelectCharacteristicsCommand = new DelegateCommand<GattCharacteristicViewModel>(ReadCharacteristics);

        }

        private void ReadCharacteristics(GattCharacteristicViewModel characteristics)
        {
            characteristics.Select();
        }

        private void ConnectionToggle()
        {
            // don't cleanup connection - force user to d/c
            if (this.peripheral.Status == ConnectionState.Disconnected)
            {
                this.peripheral.Connect();
            }
            else
            {
                this.peripheral.CancelConnection();
            }
        }

        private void PairToDevice()
        {
            var pair = _centralManager as ICanPairPeripherals;

            if (pair.PairingStatus == PairingState.Paired)
            {
                Console.WriteLine("Peripheral is already paired");
            }
            else
            {
                pair
                    .PairingRequest()
                    .Subscribe(x =>
                    {
                        var txt = x ? "Peripheral Paired Successfully" : "Peripheral Pairing Failed";
                        Console.WriteLine(txt);
                        this.RaisePropertyChanged(nameof(this.PairingText));
                    });
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.peripheral = parameters.GetValue<IPeripheral>("Peripheral");
            this.Name = this.peripheral.Name;
            this.Uuid = this.peripheral.Uuid;
            this.PairingText = this.peripheral.TryGetPairingStatus() == PairingState.Paired ? "Peripheral Paired" : "Pair Peripheral";

            RaisePropertyChanged(nameof(Name));
            RaisePropertyChanged(nameof(Uuid));
            RaisePropertyChanged(nameof(PairingText));

            this.peripheral
                .WhenReadRssiContinuously(TimeSpan.FromSeconds(3))
                .Subscribe(x => {
                    Device.BeginInvokeOnMainThread(() =>{
                        this.Rssi = x;
                        RaisePropertyChanged(nameof(Rssi));
                    });
                });


            this.peripheral
                .WhenStatusChanged()
                .Subscribe(status =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        switch (status)
                        {
                            case ConnectionState.Connecting:
                                this.ConnectText = "Cancel Connection";
                                break;

                            case ConnectionState.Connected:
                                this.ConnectText = "Disconnect";
                                break;

                            case ConnectionState.Disconnected:
                                this.ConnectText = "Connect";
                                try
                                {
                                    this.GattCharacteristics.Clear();
                                }
                                catch (Exception ex)
                                {
                                    // eat this for now
                                    Console.WriteLine(ex);
                                }

                                break;
                        }

                        RaisePropertyChanged(nameof(ConnectText));
                    });
                });

            this.peripheral
                .WhenAnyCharacteristicDiscovered()
                .Subscribe( 
                    chs =>
                    {
                        Device.BeginInvokeOnMainThread(() => {
                            var service = this.GattCharacteristics.FirstOrDefault(x => x.ShortName.Equals(chs.Service.Uuid.ToString()));
                            if (service == null)
                            {
                                service = new Group<GattCharacteristicViewModel>(
                                    $"{chs.Service.Description} ({chs.Service.Uuid})",
                                    chs.Service.Uuid.ToString()
                                );
                                this.GattCharacteristics.Add(service);
                            }

                            service.Add(new GattCharacteristicViewModel(chs));
                            RaisePropertyChanged(nameof(GattCharacteristics));
                        });  
                    },
                    ex => Console.WriteLine("ERROR (characteristic discovery) " + ex.ToString())
                );
        }
    }
}
