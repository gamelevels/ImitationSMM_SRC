using DataAccess;
using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class TicketManager : ITicketManager
    {
        private ITicketAccessor ticketAccessor = null;

        public TicketManager()
        {
            ticketAccessor = new TicketAccessor();
        }

        public bool AddTicketMessage(TicketMessages message)
        {
            try
            {
                return ticketAccessor.AddTicketMessage(message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update message", ex);
            }
        }

        public Ticket GetTicket(Guid id)
        {
            try
            {
                Ticket ticket = ticketAccessor.GetTicket(id);
                List<TicketMessages> messages = ticketAccessor.GetTicketMessages(id);
                ticket.UserMessages = messages.FindAll(x => x.IsStaff == false);
                ticket.SupportMessages = messages.FindAll(x => x.IsStaff == true);
                return ticket;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to retrieve ticket", ex);
            }
        }

        public List<Ticket> SelectUserTickets(string user)
        {
            try
            {
                return ticketAccessor.GetUserTickets(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to retrieve tickets", ex);
            }
        }

        public List<Ticket> SelectAllTickets()
        {
            try
            {
                return ticketAccessor.GetAllTickets();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to retrieve tickets", ex);
            }
        }

        public List<TicketMessages> SelectTicketMessages(Guid id)
        {
            try
            {
                return ticketAccessor.GetTicketMessages(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to retrieve ticket messages", ex);
            }
        }

        public bool UpdateTicket(Ticket ticket)
        {
            try
            {
                return ticketAccessor.UpdateTicket(ticket);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update message", ex);
            }
        }

        public bool CreateTicket(Ticket ticket)
        {
            try
            {
                return ticketAccessor.CreateTicket(ticket);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to create ticket", ex);
            }
        }
    }
}
