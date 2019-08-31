using BLEPrototype.ViewModels;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLEPrototype
{
    public class MainViewModel : BLEViewModel
    {
        private readonly INavigationService _navigationService;
        public DelegateCommand<string> NavigateCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {

            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);

        }

        private void Navigate(string navigationPath)
        {
            _navigationService.NavigateAsync(navigationPath);
        }
    }
}
