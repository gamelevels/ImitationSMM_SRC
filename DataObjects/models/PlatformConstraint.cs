using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class PlatformConstraint
    {
        public string PlatformName { get; set; }
        public int RequestCount { get; set; }
        public string RequestType { get; set; }
    }
}
