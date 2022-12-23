using System;
using System.Collections.Generic;
using System.Text;

namespace RDIApp.Models
{
    public class RateModel : CurrencyModel
    {
        public decimal Rate { get; set; }
    }

    public class RateModelView : CurrencyModel
    {
        public decimal FirstRate { get; set; }
        public decimal SecondRate { get; set; }
        public int Position { get; set; }
    }
}
