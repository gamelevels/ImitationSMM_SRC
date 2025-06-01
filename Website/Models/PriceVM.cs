using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class PriceVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Concurrents { get; set; }
        public int Cooldown { get; set; }
        public int Level { get; set; }
        public string Platforms { get; set; }
    }
}