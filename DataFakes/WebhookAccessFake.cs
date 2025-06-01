using DataInterfaces;
using DataObjects.models;
using System.Collections.Generic;

namespace DataFakes
{
    public class WebhookAccessFake : IWebhookAccessor
    {
        List<Webhook> webhooks = new List<Webhook>();
        public WebhookAccessFake()
        {
            webhooks.Add(new Webhook() { Link = "https://www.discord.com/xxx", Title = "Title1", Enabled = true });
            webhooks.Add(new Webhook() { Link = "https://www.discord.com/yyy", Title = "Title2", Enabled = false });
            webhooks.Add(new Webhook() { Link = "https://www.discord.com/zzz", Title = "Title3", Enabled = true });
        }
        public List<Webhook> SelectWebhooks()
        {
            return webhooks;
        }

        public bool UpdateWebhook(Webhook webhook)
        {
            try
            {
                Webhook wh = webhooks.Find(w => w.Link == webhook.Link);
                wh = webhook;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
