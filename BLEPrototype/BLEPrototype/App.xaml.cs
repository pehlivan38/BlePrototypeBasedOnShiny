using Prism.Unity;
using Prism.Ioc;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity.Extensions;
using Prism.Mvvm;
using BLEPrototype.BluetoothLE;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BLEPrototype
{
    public partial class App : PrismApplication
    {
        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;

        protected override void OnInitialized()
        {
            InitializeComponent();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = viewType.FullName.Replace("Page", "ViewModel");
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
            NavigationService.NavigateAsync("Main/Nav/About");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("Nav");
            containerRegistry.RegisterForNavigation<MainPage>("Main");
            containerRegistry.RegisterForNavigation<AboutPage>("About");

            containerRegistry.RegisterForNavigation<AdapterPage>("BleCentral");
        }
    }
}
