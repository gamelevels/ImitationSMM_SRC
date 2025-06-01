using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            string plat1 = "Tiktok, Instagram";
            string plat2 = $"{plat1}, Twitter, Youtube";
            List<PriceVM> list = new List<PriceVM>
            {
                new PriceVM { Name = "Basic", Price = 5, Concurrents = 1, Cooldown = 80, Level = 1, Platforms = plat1 },
                new PriceVM { Name = "Casual", Price = 10, Concurrents = 1, Cooldown = 70, Level = 2, Platforms = plat1 },
                new PriceVM { Name = "Normal", Price = 15, Concurrents = 1, Cooldown = 60, Level = 3, Platforms = plat1 },
                new PriceVM { Name = "Beta", Price = 20, Concurrents = 2, Cooldown = 50, Level = 4, Platforms = plat1 },
                new PriceVM { Name = "Alpha", Price = 25, Concurrents = 2, Cooldown = 40, Level = 5, Platforms = plat2 },
                new PriceVM { Name = "Elite", Price = 35, Concurrents = 2, Cooldown = 30, Level = 6, Platforms = plat2 },
                new PriceVM { Name = "Gamma", Price = 45, Concurrents = 3, Cooldown = 20, Level = 7, Platforms = plat2 },
                new PriceVM { Name = "Custom", Price = 999, Concurrents = 999, Cooldown = 999, Level = 8, Platforms = plat2 },
                new PriceVM { Name = "Admin", Price = 75, Concurrents = 3, Cooldown = 10, Level = 9, Platforms = plat2 },
                new PriceVM { Name = "Dev", Price = 10800, Concurrents = 12, Cooldown = 0, Level = 10, Platforms = plat2 }
            };

            return View(list);
        }
        public ActionResult Features()
        {
            return View();
        }
        public ActionResult TOS()
        {
            return View();
        }


    }
}