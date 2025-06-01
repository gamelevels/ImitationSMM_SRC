using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class UserHistory
    {
        public string Username { get; set; }
        public string RequestType { get; set; }
        public string RequestHandle { get; set; }
        public string RequestDetails { get; set; }
        public DateTime Date { get; set; }
    }
}
