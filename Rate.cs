using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConverter
{
    public class Rate
    {
        public double USD { get; set; }
        public double EUR { get; set; }
        public double GBP { get; set; }
        public double JPY { get; set; }
        public double AUD { get; set; }
        public double CAD { get; set; }
        public double CHF { get; set; }
        public double CNY { get; set; }
        public double INR { get; set; }

        
        
    }
}
