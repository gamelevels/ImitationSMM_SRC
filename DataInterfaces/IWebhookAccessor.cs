using DataObjects.models;
using System.Collections.Generic;

namespace DataInterfaces
{
    public interface IWebhookAccessor
    {
        List<Webhook> SelectWebhooks();
        bool UpdateWebhook(Webhook webhook);
    }
}
