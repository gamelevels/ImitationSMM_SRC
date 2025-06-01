using DataObjects;
using DataObjects.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class DiscordUtils
    {
        public static string DiscordInvite = "https://discord.gg/TACUNJJNGg";

        public async static Task<bool> LogToDiscord(string action, string msg)
        {
            action = action.ToLower();
            string logTitle = $"ImitationSMM - {action}";
            string logUrl = MainManager.GetMainManager().WebhookManager.GetWebhooks().First(w => w.Title.ToLower() == action).Link;
            var newToken = Guid.NewGuid().ToString();
            var token = newToken.Substring(newToken.Length - 6);
            DiscordLog log = new DiscordLog();


            DiscordEmbed embed = new DiscordEmbed();
            embed.title = $"[{token}] Notifcation";
            embed.color = 16711680;
            embed.fields = new List<DiscordField>
            {
                new DiscordField(
                    name: logTitle,
                    value: $"`{msg}`",
                    inline: true
                ),
                new DiscordField(
                    name: "Time Stamp",
                    value: new DateTimeOffset(DateTime.UtcNow).LocalDateTime.ToString()
                )
            };


            log.username = "ImitationSMM";
            log.avatar_url = "https://i.imgur.com/wzh4lAn.png";
            log.embeds = new List<DiscordEmbed> {
                embed
            };

            return await PostRequest(logUrl, log);

        }
        private static async Task<bool> PostRequest(string url, DiscordLog log)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    StringContent jsonContent = new StringContent(
                        JsonConvert.SerializeObject(log),
                        Encoding.UTF8,
                        "application/json"
                    );

                    HttpResponseMessage req = await client.PostAsync(url, jsonContent);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private class DiscordField
        {
            public string name { get; set; }
            public string value { get; set; }
            public bool inline { get; set; }
            public DiscordField(string name, string value, bool inline = false)
            {
                this.name = name;
                this.value = value;
                this.inline = inline;
            }
        }

        private class DiscordEmbed
        {
            public string title { get; set; }
            public int color { get; set; }
            public List<DiscordField> fields { get; set; }
        }

        private class DiscordLog
        {
            public string username { get; set; }
            public string avatar_url { get; set; }
            public List<DiscordEmbed> embeds { get; set; }
        }
    }
}
