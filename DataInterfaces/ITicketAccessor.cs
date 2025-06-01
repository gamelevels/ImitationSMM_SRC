using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterfaces
{
    public interface ITicketAccessor
    {
        List<TicketMessages> GetTicketMessages(Guid id);
        List<Ticket> GetAllTickets();
        Ticket GetTicket(Guid id);
        bool UpdateTicket(Ticket ticket);
        bool AddTicketMessage(TicketMessages message);
        List<Ticket> GetUserTickets(string user);
        bool CreateTicket(Ticket ticket);
    }
}
