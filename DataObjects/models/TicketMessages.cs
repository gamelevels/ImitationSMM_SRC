using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class TicketMessages
    {
        public Guid TicketID { get; set; }
        public DateTime SentTime { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public bool IsStaff { get; set; }
    }
}
