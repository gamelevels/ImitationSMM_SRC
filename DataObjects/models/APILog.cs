using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class APILog
    {
        public string Link { get; set; }
        public string UsedBy { get; set; }
        public string Response { get; set; }
        public DateTime Date { get; set; }
    }
}
