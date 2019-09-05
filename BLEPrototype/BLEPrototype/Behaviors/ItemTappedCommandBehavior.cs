using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BLEPrototype.Behaviors
{
    public class ItemTappedCommandBehavior : AbstractBindableBehavior<ListView>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ItemTappedCommandBehavior));
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }


        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            this.AssociatedObject.ItemTapped += this.OnItemTapped;
        }


        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            this.AssociatedObject.ItemTapped -= this.OnItemTapped;
        }


        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (this.Command == null || e.Group == null)
                return;

            if (this.Command.CanExecute(e.Group))
                this.Command.Execute(e.Group);
        }
    }
}
