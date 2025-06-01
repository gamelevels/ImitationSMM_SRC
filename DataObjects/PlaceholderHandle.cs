using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class PlaceholderHandle
    {
        public static Dictionary<string, string> URLS = new Dictionary<string, string>()
        {
            ["Instagram"] = "https://www.instagram.com/p/abcdeFGH/",
            ["Facebook"] = "https://www.facebook.com/groups/paintball/12344321",
            ["Twitter/X"] = "https://www.twitter.com/Username/status/123321123",
            ["YouTube"] = "https://www.youtube.com/watch?v=321332132131",
            ["TikTok"] = "https://www.tiktok.com/@username/video/123321123"
        };
    }
}
