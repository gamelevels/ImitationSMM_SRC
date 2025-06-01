using DataObjects.models;
using LogicInterfaces;
using System;

namespace Logic
{
    /// <summary>
    /// Store all managers, and logged in user
    /// </summary>
    public class MainManager
    {
        public UserVM LoggedInUser { get; set; }

        private Lazy<IAPIManager> apiManager = new Lazy<IAPIManager>(() => new APIManager());
        private Lazy<IApplicationManager> applicationManager = new Lazy<IApplicationManager>(() => new ApplicationManager());
        private Lazy<ILogManager> logManager = new Lazy<ILogManager>(() => new LogManager());
        private Lazy<IPlatformManager> platformManager = new Lazy<IPlatformManager>(() => new PlatformManager());
        private Lazy<IUserManager> userManager = new Lazy<IUserManager>(() => new UserManager());
        private Lazy<IWebhookManager> webhookManager = new Lazy<IWebhookManager>(() => new WebhookManager());
        private Lazy<ITicketManager> ticketManager = new Lazy<ITicketManager>(() => new TicketManager());
        public IAPIManager APIManager => apiManager.Value;
        public IApplicationManager ApplicationManager => applicationManager.Value;
        public ILogManager LogManager => logManager.Value;
        public IPlatformManager PlatformManager => platformManager.Value;
        public IUserManager UserManager => userManager.Value;
        public IWebhookManager WebhookManager => webhookManager.Value;
        public ITicketManager TicketManager => ticketManager.Value;

        private MainManager() { }

        private static MainManager instance;
        public static MainManager GetMainManager()
        {
            return instance ?? (instance = new MainManager());
        }
    }
}
