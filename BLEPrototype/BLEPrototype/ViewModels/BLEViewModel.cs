using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLEPrototype.ViewModels
{
    public abstract class BLEViewModel : BindableBase,
                                        IAutoInitialize,
                                        IInitialize,
                                        IInitializeAsync,
                                        IPageLifecycleAware//,
                                                   //IDestructible,
                                                   //IConfirmNavigationAsync
    {
        public virtual void Initialize(INavigationParameters parameters)
        {
            
        }

        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;

        public virtual void OnAppearing()
        {
            
        }

        public virtual void OnDisappearing()
        {
            
        }
    }
}
