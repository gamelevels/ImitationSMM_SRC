using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    public class AppInformation
    {
        public string MOTD { get; set; }
        public string AppVersion { get; set; }
        public string TOS { get; set; }
        public string Help { get; set; }
        public List<Webhook> Webhooks = new List<Webhook>();
        public int? UserCount { get; set; }
        public int? AccountsCount { get; set; } = 0;
        public string PopularPlatform { get; set; }

    }
}
