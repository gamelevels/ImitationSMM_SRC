using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class Captcha
    {
        // Wasn't built for this, so hard coding -- obv would be hidden in prod
        public static string SiteKey = "";
        private static string SecretKey = "";
        public static string Endpoint = "https://www.google.com/recaptcha/api/siteverify?secret=" + SecretKey;
        public static async Task<bool> SolveCaptcha(string response)
        {
            var resp = await RequestCaptcha(response);
            return resp.IsSuccessStatusCode && await Validate(resp);
        }

        private static async Task<bool> Validate(HttpResponseMessage resp)
        {
            var content = await resp.Content.ReadAsStringAsync();
            return content.Contains("\"success\": true");
        }

        private static async Task<HttpResponseMessage> RequestCaptcha(string response)
        {
            return await new HttpClient().GetAsync($"{Endpoint}&response={response}");
        }
    }
}
