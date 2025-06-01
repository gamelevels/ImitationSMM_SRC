using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInterfaces;
using System.Net.Sockets;

namespace DataAccess
{
    public class TicketAccessor : ITicketAccessor
    {
        public List<Ticket> GetAllTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            using (var con = SqlUtils.NewConnection())
            using (SqlCommand cmd = SqlUtils.NewCommand("sp_select_all_tickets", con, CommandType.StoredProcedure))
            {
                try
                {
                    con.Open();
                    using (SqlDataReader dbResult = cmd.ExecuteReader())
                    {
                        while (dbResult.Read())
                        {
                            tickets.Add(new Ticket()
                            {
                                ID = dbResult.GetGuid(0),
                                Title = dbResult.GetString(1),
                                Issue = dbResult.GetString(2),
                                Priority = dbResult.GetInt32(3),
                                Resolved = dbResult.GetBoolean(4),
                                Submitter = dbResult.GetString(5),
                                DateSubmitted = dbResult.GetDateTime(6)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return tickets;
        }

        public List<Ticket> GetUserTickets(string user)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (var con = SqlUtils.NewConnection())
            using (SqlCommand cmd = SqlUtils.NewCommand("sp_select_users_tickets", con, CommandType.StoredProcedure))
            {
                cmd.Parameters.AddWithValue("@User", user);
                try
                {
                    con.Open();
                    using (SqlDataReader dbResult = cmd.ExecuteReader())
                    {
                        while (dbResult.Read())
                        {
                            tickets.Add(new Ticket()
                            {
                                ID = dbResult.GetGuid(0),
                                Title = dbResult.GetString(1),
                                Issue = dbResult.GetString(2),
                                Priority = dbResult.GetInt32(3),
                                Resolved = dbResult.GetBoolean(4),
                                Submitter = dbResult.GetString(5),
                                DateSubmitted = dbResult.GetDateTime(6)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return tickets;
        }

        public Ticket GetTicket(Guid id)
        {
            Ticket ticket = null;
            using (var con = SqlUtils.NewConnection())
            using (SqlCommand cmd = SqlUtils.NewCommand("sp_select_ticket", con, CommandType.StoredProcedure))
            {
                cmd.Parameters.AddWithValue("@TicketID", id);
                try
                {
                    con.Open();
                    using (SqlDataReader dbResult = cmd.ExecuteReader())
                    {
                        while (dbResult.Read())
                        {
                            ticket = new Ticket()
                            {
                                ID = dbResult.GetGuid(0),
                                Title = dbResult.GetString(1),
                                Issue = dbResult.GetString(2),
                                Priority = dbResult.GetInt32(3),
                                Resolved = dbResult.GetBoolean(4),
                                Submitter = dbResult.GetString(5),
                                DateSubmitted = dbResult.GetDateTime(6)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return ticket;
        }

        public List<TicketMessages> GetTicketMessages(Guid id)
        {
            List<TicketMessages> messages = new List<TicketMessages>();
            using (var con = SqlUtils.NewConnection())
            using (SqlCommand cmd = SqlUtils.NewCommand("sp_select_ticket_messages", con, CommandType.StoredProcedure))
            {
                cmd.Parameters.AddWithValue("@TicketID", id);
                try
                {
                    con.Open();
                    using (SqlDataReader dbResult = cmd.ExecuteReader())
                    {
                        while (dbResult.Read())
                        {
                            messages.Add(new TicketMessages()
                            {
                                TicketID = dbResult.GetGuid(0),
                                Message = dbResult.GetString(1),
                                Sender = dbResult.GetString(2),
                                SentTime = dbResult.GetDateTime(3),
                                IsStaff = dbResult.GetBoolean(4)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return messages;
        }

        public bool CreateTicket(Ticket ticket)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_create_ticket", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@ID", ticket.ID);
                cmd.Parameters.AddWithValue("@Title", ticket.Title);
                cmd.Parameters.AddWithValue("@Issue", ticket.Issue);
                cmd.Parameters.AddWithValue("@Priority", ticket.Priority);
                cmd.Parameters.AddWithValue("@Submitter", ticket.Submitter);

                try
                {
                    con.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rows == 1;
        }

        public bool UpdateTicket(Ticket ticket)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_update_ticket", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@TicketID", ticket.ID);
                cmd.Parameters.AddWithValue("@Title", ticket.Title);
                cmd.Parameters.AddWithValue("@Priority", ticket.Priority);
                cmd.Parameters.AddWithValue("@Resolved", ticket.Resolved);

                try
                {
                    con.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rows == 1;
        }

        public bool AddTicketMessage(TicketMessages message)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_add_ticket_message", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@TicketID", message.TicketID);
                cmd.Parameters.AddWithValue("@Message", message.Message);
                cmd.Parameters.AddWithValue("@Sender", message.Sender);
                cmd.Parameters.AddWithValue("@IsStaff", message.IsStaff);

                try
                {
                    con.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rows == 1;
        }
    }
}
