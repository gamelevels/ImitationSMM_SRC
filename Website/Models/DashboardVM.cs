using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class DashboardVM
    {
        public int UserCount { get; set; }
        public int SMMAccounts { get; set; }
        public int OnlineUsers { get; set; } // Probably wont implement
        public string MOTD { get; set; }
        public string UpdateLog { get; set; }
        public bool Popup {  get; set; }
    }
}