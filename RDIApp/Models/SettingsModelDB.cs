using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDIApp.Models
{
    public class SettingsModelDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
        public int NumCode { get; set; }
        public int Scale { get; set; }
        public string CharCode { get; set; }
        public string Description { get; set; }
    }
}
