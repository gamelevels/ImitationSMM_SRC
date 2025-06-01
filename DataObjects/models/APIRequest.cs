using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class APIRequest
    {
        public string URL { get; set; }
        public string Handle { get; set; }
        public string FillType { get; set; }
        public string Platform { get; set; }
        public int Requests { get; set; }
        public bool ToS { get; set; }
        public string History { get; set; }
    }
}
