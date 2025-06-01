using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class WebhookAccessor : IWebhookAccessor
    {
        public List<Webhook> SelectWebhooks()
        {
            List<Webhook> webhooks = new List<Webhook>();

            using(var con = SqlUtils.NewConnection())
            using(var cmd = SqlUtils.NewCommand("sp_select_webhooks", con, CommandType.StoredProcedure))
            {
                con.Open();
                using (SqlDataReader dbResult = cmd.ExecuteReader())
                {
                    while (dbResult.Read())
                    {
                        Webhook webhook = new Webhook()
                        {
                            Link = dbResult.GetString(0),
                            Title = dbResult.GetString(1),
                            Enabled = dbResult.GetBoolean(2)
                        };
                        webhooks.Add(webhook);
                    }
                }
            }

            return webhooks;
        }


        public bool UpdateWebhook(Webhook webhook)
        {
            int rows = 0;
            using(var con = SqlUtils.NewConnection())
            using(SqlCommand cmd = SqlUtils.NewCommand("sp_update_webhook", con, CommandType.StoredProcedure))
            {

                cmd.Parameters.AddWithValue("@Link", webhook.Link);
                cmd.Parameters.AddWithValue("@Title", webhook.Title);
                cmd.Parameters.AddWithValue("@Enabled", webhook.Enabled);

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
