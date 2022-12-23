using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDIApp.Models
{
    public class SettingsModel : CurrencyModel
    {
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
