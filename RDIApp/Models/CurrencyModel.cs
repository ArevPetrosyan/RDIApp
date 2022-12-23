using RDIApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RDIApp.Models
{
    public class CurrencyModel
    {
        public int Id { get; set; }
        public int NumCode { get; set; }
        public int Scale { get; set; }
        public string CharCode { get; set; }
        public string Description { get; set; }
    }        
}
