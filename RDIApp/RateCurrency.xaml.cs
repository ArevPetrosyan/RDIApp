using RDIApp.Models;
using RDIApp.Services;
using RDIApp.ViewModels;
using RDIApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RDIApp
{
    public partial class RateCurrency : ContentPage
    {
        public RateCurrency()
        {
            InitializeComponent();
            BindingContext = new RateCurrencyViewModel();
        }              

        private async void OpenSettings_Onclick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingsView());
        }
    }
}
