using RDIApp.Models;
using RDIApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RDIApp.Services
{
    public static class DataProviderService
    {
        public static async Task<List<RateModel>> GetRates(DateTime date)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format($"http://www.nbrb.by/Services/XmlExRates.aspx?ondate={date.ToString("MM/dd/yyyy")}", string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                XDocument xDocument = XDocument.Parse(content);
                var studentLst = xDocument.Descendants("DailyExRates");
                if (studentLst != null)
                {
                    var rateModels = studentLst.Descendants("Currency").Select(c2 =>
                    new RateModel
                    {
                        NumCode = int.Parse(c2.Element("NumCode").Value),
                        CharCode = c2.Element("CharCode").Value,
                        Description = c2.Element("Name").Value,
                        Scale = int.Parse(c2.Element("Scale").Value),
                        Rate = decimal.Parse(c2.Element("Rate").Value)
                    }).ToList<RateModel>();

                    return rateModels;
                }
            }
            return null;
        }

        public static void CreateDefSettings(List<RateModel> items)
        {
            AppDataState.AppSettings = new Dictionary<string, SettingsModelDB>();
            App.Database.DeleteItems();
            int i = 4;
            foreach (var rate in items)
            {
                int p = 1;
                switch (rate.CharCode)
                {
                    case "USD":
                        p = 1;
                        break;
                    case "RUB":
                        p = 2;
                        break;
                    case "EUR":
                        p = 3;
                        break;
                }
                var item = new SettingsModelDB
                {
                    Description = rate.Description,
                    CharCode = rate.CharCode,
                    Scale = rate.Scale,
                    NumCode = rate.NumCode,
                    IsActive = rate.CharCode == "USD" || rate.CharCode == "RUB" || rate.CharCode == "EUR",
                    Position = (rate.CharCode == "USD" || rate.CharCode == "RUB" || rate.CharCode == "EUR") ? p : i
                };

                App.Database.InsertSettingsAsync(item);

                AppDataState.AppSettings.Add(rate.CharCode, item);

                if (rate.CharCode != "USD" && rate.CharCode != "RUB" && rate.CharCode != "EUR")
                    i++;
            }
        }

        public static void EditSettings(ItemsListViewModel settings)
        {
            App.Database.DeleteItems();

            foreach (var item in settings)
            {
                App.Database.InsertSettingsAsync(new SettingsModelDB
                {
                    Description = item.Description,
                    CharCode = item.CharCode,
                    IsActive = item.IsActive,
                    NumCode = item.NumCode,
                    Position = item.Position,
                    Scale = item.Scale
                });
            }
        }

        public static List<RateModelView> Mapdata(List<RateModel> firstRateData, List<RateModel> secondRateData)
        {
            var Items = new List<RateModelView>();
            foreach (var rate in firstRateData)
            {
                if (AppDataState.AppSettings[rate.CharCode].IsActive)
                {
                    var r = secondRateData.FirstOrDefault(x => x.NumCode == rate.NumCode).Rate;
                    Items.Add(new RateModelView
                    {
                        CharCode = rate.CharCode,
                        Position = AppDataState.AppSettings[rate.CharCode].Position,
                        NumCode = rate.NumCode,
                        Description = rate.Description,
                        Scale = rate.Scale,
                        Id = rate.Id,
                        FirstRate = rate.Rate,
                        SecondRate = r
                    });
                }
            }

            return Items.OrderBy(x => x.Position).ToList();
        }

        public static void UpdateSettings()
        {
            AppDataState.AppSettings = new Dictionary<string, SettingsModelDB>();
            var list = App.Database.GetSettings().Result;
            foreach (var item in list)
                AppDataState.AppSettings.Add(item.CharCode, item);
        }
    }
}
