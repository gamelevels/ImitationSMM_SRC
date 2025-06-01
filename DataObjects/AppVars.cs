using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public static class AppVars
    {
        public static AppInformation appInfo { get; set; }

        // Only hard coding this because I didn't plan for a stats link
        // Don't want rewrite the current api logic
        public static Uri StatsURL = new Uri("http://127.0.0.1:8080/stats/v1/");
        public static Uri BalanceURL = new Uri("http://127.0.0.1:8080/stats/balance/?key=...");
    }
}
