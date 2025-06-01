using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    // stupid ide underlining
#pragma warning disable SYSLIB0014 
    public class ExtendedWebClient : WebClient
    {
        // needed to add a timeout instead of hanging up on bad connection
        public int Timeout { get; set; } = 2000;

        public new async Task<string> DownloadStringTaskAsync(Uri address)
        {
            var t = base.DownloadStringTaskAsync(address);
            if (await Task.WhenAny(t, Task.Delay(Timeout)) != t) 
            {
                CancelAsync();
            }
            return await t;
        }
    }
#pragma warning restore SYSLIB0014 
}
