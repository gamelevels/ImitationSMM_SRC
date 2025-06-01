using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class Ticket
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please provide a description")]
        public string Issue { get; set; }
        [Required(ErrorMessage = "Priority is required")]
        public int Priority { get; set; }
        public bool Resolved { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Submitter { get; set; }
        public List<TicketMessages> SupportMessages { get; set; }
        public List<TicketMessages> UserMessages { get; set; }
    }
}
