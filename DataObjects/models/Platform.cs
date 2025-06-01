using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class Platform
    {
        public string PlatformName { get; set; }
        public int MinimumLevel { get; set; }
        public bool Enabled { get; set; }
    }
}
