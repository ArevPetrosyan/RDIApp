using RDIApp.Models;
using System.Collections.Generic;

namespace RDIApp.Services
{
    public static class AppDataState
    {
        private static Dictionary<string, SettingsModelDB> appSettings;
        public static Dictionary<string, SettingsModelDB> AppSettings
        {
            get
            {
                if (appSettings == null)
                    return new Dictionary<string, SettingsModelDB>();
                else return appSettings;
            }
            set
            {
                appSettings = value;
            }
        }
    }
}
