using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class UserLog
    {
        public string Username { get; set; }
        public string LogType { get; set; }
        public string LogDetails { get; set; }
        public DateTime? Date { get; set; }
    }
}
