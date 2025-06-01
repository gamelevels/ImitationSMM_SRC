using DataAccess;
using DataInterfaces;
using DataObjects.models;
using LogicInterfaces;
using System;
using System.Collections.Generic;

namespace Logic
{
    public class WebhookManager : IWebhookManager
    {
        private IWebhookAccessor webookAccessor = null;

        public WebhookManager()
        {
            webookAccessor = new WebhookAccessor();
        }

        public WebhookManager(IWebhookAccessor access)
        {
            webookAccessor = access;
        }
        public List<Webhook> GetWebhooks()
        {
            try
            {
                return webookAccessor.SelectWebhooks();
            }
            catch
            {
                throw new ApplicationException("Error retrieving webhooks");
            }
        }

        public bool UpdateWebhook(Webhook webhook)
        {
            try
            {
                return webookAccessor.UpdateWebhook(webhook);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update webhook", ex);
            }
        }
    }
}
