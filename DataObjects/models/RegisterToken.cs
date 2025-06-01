using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class RegisterToken
    {
        public Guid Token { get; set; }
        public int Level { get; set; }
        public int Days { get; set; }
        public string UsedBy { get; set; }
        public DateTime UsedDate { get; set; }
    }
}
