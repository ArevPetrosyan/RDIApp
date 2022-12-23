using RDIApp.Models;
using RDIApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace RDIApp.ViewModels
{
    public class RateCurrencyViewModel : BaseModelView
    {
        private ObservableCollection<RateModelView> rateList { get; set; }

        public ObservableCollection<RateModelView> RateList
        {
            get { return rateList; }
            set
            {
                rateList = value;
                OnPropertyChanged("RateList");
            }
        }


        public RateCurrencyViewModel()
        {
            RefreshCommand = new Command(RefreshData);
            Title = "Курсы валют";
            BackIsVisible = false;
            InitData();
        }

        public ICommand RefreshCommand { get; }

        private async void RefreshData()
        {
            IsRefreshing = true;
            InitData();
            IsRefreshing = false;
        }

        private bool isRefreshing { get; set; }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        private bool backIsVisible { get; set; }
        public bool BackIsVisible
        {
            get { return backIsVisible; }
            set
            {
                backIsVisible = value;
                OnPropertyChanged("BackIsVisible");
            }
        }

        private bool settingsIsVisible { get; set; }
        public bool SettingsIsVisible
        {
            get { return settingsIsVisible; }
            set
            {
                settingsIsVisible = value;
                OnPropertyChanged("SettingsIsVisible");
            }
        }

        private string firstDate { get; set; }
        public string FirstDate
        {
            get { return firstDate; }
            set
            {
                firstDate = value;
                OnPropertyChanged("FirstDate");
            }
        }

        private string title { get; set; }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private string secondDate { get; set; }
        public string SecondDate
        {
            get { return secondDate; }
            set
            {
                secondDate = value;
                OnPropertyChanged("SecondDate");
            }
        }

        private View prevView { get; set; }
        public View PrevView
        {
            get { return prevView; }
            set
            {
                prevView = value;
                OnPropertyChanged("PrevView");
            }
        }

        private View currentView { get; set; }
        public View CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        private async void InitData()
        {
            var ratestoday = await DataProviderService.GetRates(DateTime.Now);

            if (ratestoday?.Count > 0)
            {
                if (AppDataState.AppSettings.Count == 0)
                    DataProviderService.CreateDefSettings(ratestoday);

                DataProviderService.UpdateSettings();
                SettingsIsVisible = true;
                var ratesTomorrow = await DataProviderService.GetRates(DateTime.Now.AddDays(1));
                if (ratesTomorrow?.Count == 0)
                {
                    var ratesYesterday = await DataProviderService.GetRates(DateTime.Now.AddDays(-1));
                    var list = DataProviderService.Mapdata(ratesYesterday, ratestoday);
                    RateList = new ObservableCollection<RateModelView>(list);
                    FirstDate = DateTime.Now.AddDays(-1).ToString("dd.MM.yyy");
                    SecondDate = DateTime.Now.ToString("dd.MM.yyy");
                }
                else
                {
                    FirstDate = DateTime.Now.ToString("dd.MM.yyy");
                    SecondDate = DateTime.Now.AddDays(1).ToString("dd.MM.yyy");
                    var list = DataProviderService.Mapdata(ratestoday, ratesTomorrow);
                    RateList = new ObservableCollection<RateModelView>(list);
                }
            }
        }
    }
}
