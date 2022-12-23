using RDIApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RDIApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        CurrencySettingsViewModel model;
        public SettingsView()
        {
            InitializeComponent();
            BindingContext = new CurrencySettingsViewModel();
        }

        private void DragGestureRecognizer_DragStarting_Collection(System.Object sender, Xamarin.Forms.DragStartingEventArgs e)
        {

        }

        private void DropGestureRecognizer_Drop_Collection(System.Object sender, Xamarin.Forms.DropEventArgs e)
        {
            e.Handled = true;
        }

        private void BackTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RateCurrency());
        }
    }
}