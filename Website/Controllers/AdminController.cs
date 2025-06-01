using DataObjects.models;
using Logic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    [LevelAuthorization(10)]
    public class AdminController : Controller
    {
        private ApplicationUserManager userManager;
        private MainManager manager = MainManager.GetMainManager();

        public ActionResult Index()
        {
            List<User> users = manager.UserManager.GetUsers();
            return View(users.OrderBy(u => u.Username));
        }

        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    ApplicationUser applicationUser = userManager.FindById(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var lvls = Enumerable.Range(1, 10).ToList();
        //    lvls.Remove(applicationUser.Level);
        //    ViewBag.Levels = lvls;

        //    return View(applicationUser);
        //}

        public ActionResult Edit(Guid? token)
        {
            if(token == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            try
            {
                manager = MainManager.GetMainManager();
                var regToken = manager.UserManager.GetToken(token.Value);
                var user = manager.UserManager.GetUser(regToken.UsedBy);
                return View(user);
            }
            catch
            {
                return View(new User());
            }
        }

        public ActionResult TokenGen()
        {
            try
            {
                List<RegisterToken> tokens = Enumerable.Range(1, 20).Select((_, index) => new RegisterToken()
                {
                    Token = Guid.NewGuid(),
                    // lvl 1,2,3 + 10
                    Level = index % 4 == 0 ? 10 : index % 3 + 1,
                    Days = 2000
                }).ToList();

                var manager = MainManager.GetMainManager();
                tokens.ForEach(t =>
                {
                    manager.UserManager.AddRegisterToken(t);
                });

                return View(tokens);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                manager = MainManager.GetMainManager();
                var user = manager.UserManager.FindUser(model.Username);
                if (user != null)
                {
                    user.Expiration = model.Expiration;
                    user.LevelInfo.Level = model.LevelInfo.Level;
                    if (!(new int[] { 1, 2, 3, 4, 10 }.Contains(user.LevelInfo.Level)))
                    {
                        return View(model);
                    }
                    user.Enabled = model.Enabled;
                    try
                    {
                        // Commenting out instead of trying to debug microsofts' update method
                        var one = manager.UserManager.UpdateUser(model);
                        //var two = userManager.Update(user).Succeeded;
                        //if (one && two)
                        if(one)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return View(model);
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public async Task<ActionResult> APIs()
        {
            List<API> apis = new List<API>();
            var manager = MainManager.GetMainManager();
            try
            {
                var rand = new Random();
                apis = manager.APIManager.GetAPIs();
                // Wasn't built to store these, hard coding for display
                // Purposes -- no .env
                apis.ForEach(ap =>
                {
                    ap.Name = "ThunderSMM";
                    ap.Key = ""; // hidden for push obv
                    ap.Key = new string('*', ap.Key.Length - 15) + ap.Key.Substring(ap.Key.Length - 6);
                    ap.ActiveRequests = rand.Next(0, 3);
                    ap.TotalRequests = rand.Next(50, 300);
                });
            }
            catch { }

            try
            {
                decimal bal = await manager.APIManager.GetAPIBalance();
                apis.ForEach(api =>
                {
                    api.Balance = bal;
                });
            }
            catch { }

            return View(apis);
        }
    }
}
