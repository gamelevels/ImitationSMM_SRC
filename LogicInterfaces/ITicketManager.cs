using DataObjects.models;
using System;
using System.Collections.Generic;

namespace Logic
{
    public interface ITicketManager
    {
        List<Ticket> SelectAllTickets();
        List<TicketMessages> SelectTicketMessages(Guid id);
        Ticket GetTicket(Guid id);
        bool UpdateTicket(Ticket ticket);
        bool AddTicketMessage(TicketMessages message);
        List<Ticket> SelectUserTickets(string user);
        bool CreateTicket(Ticket ticket);
    }
}