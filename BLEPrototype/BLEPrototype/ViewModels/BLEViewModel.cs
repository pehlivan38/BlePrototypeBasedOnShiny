using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace BLEPrototype.ViewModels
{
    public abstract class BLEViewModel : BindableBase,
                                        IAutoInitialize,
                                        IInitialize,
                                        IInitializeAsync,
                                        INavigatedAware,
                                        IPageLifecycleAware,
                                        IDestructible,
                                        IConfirmNavigationAsync
    {
        
        #region IDestructible
        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();

        

        public virtual void Destroy() => this.DestroyWith?.Dispose();

        #endregion

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

        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);

        CompositeDisposable deactivateWith;
        protected CompositeDisposable DeactivateWith
        {
            get
            {
                if (this.deactivateWith == null)
                    this.deactivateWith = new CompositeDisposable();

                return this.deactivateWith;
            }
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            this.deactivateWith?.Dispose();
            this.deactivateWith = null;
        }
    }
}
