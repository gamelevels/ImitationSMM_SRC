using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string Submitter { get; set; }
        public string Message { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
