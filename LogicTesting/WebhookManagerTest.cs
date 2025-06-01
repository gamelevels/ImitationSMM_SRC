using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects;
using DataFakes;
using LogicInterfaces;
using Logic;
using DataObjects.models;
using System.Collections.Generic;

namespace LogicTesting
{
    [TestClass]
    public class WebhookManagerTest
    {
        IWebhookManager webhookManager = null;
        public WebhookManagerTest()
        {
            webhookManager = new WebhookManager(new WebhookAccessFake());
        }

        [TestMethod]
        public void TestReturnsValidWebhooks()
        {
            List<Webhook> webhooks = webhookManager.GetWebhooks();
            Assert.IsNotNull(webhooks);
            Assert.AreEqual(3, webhooks.Count);
        }

        [TestMethod] 
        public void TestReturnsInvalidWebhooks()
        {
            Webhook webhook = new Webhook()
            {
                Link = "https://www.discord.com/xxx",
                Title = "testing3",
                Enabled = false
            };

            bool result = webhookManager.UpdateWebhook(webhook);
            Assert.AreEqual(true, result);
        }
    }
}