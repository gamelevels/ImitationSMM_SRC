using DataObjects;
using DataObjects.models;
using Logic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.WebSockets;
using Website.Models;

namespace Website.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private MainManager manager = MainManager.GetMainManager();

        public ActionResult Index()
        {
            return View();
        }

        private async Task<DashboardVM> LoadDashboard()
        {
            DashboardVM vm;
            try
            {
                var appInfo = manager.ApplicationManager.SetApplicationInformation();
                appInfo.MOTD = appInfo.MOTD.Replace("\\n", Environment.NewLine);

                int accs = await manager.APIManager.GetAccountCount();
                vm = new DashboardVM()
                {
                    UserCount = manager.ApplicationManager.GetUserCount(),
                    //OnlineUsers = 1, // prob will implement
                    SMMAccounts = accs,
                    MOTD = appInfo.MOTD,
                    UpdateLog = "To\nBe\nImplemented"
                };
            }
            catch
            {
                vm = new DashboardVM()
                {
                    UserCount = manager.ApplicationManager.GetUserCount(),
                    //OnlineUsers = 1,
                    SMMAccounts = 0,
                    MOTD = "Unable to retrieve MOTD",
                    UpdateLog = "Unable to retrieve update log"
                };
            }
            return vm;
        }

        public async Task<ActionResult> Dashboard()
        {
            return View(await LoadDashboard());
        }

        private SMMVM LoadUserData()
        {
            SMMVM mv;
            var userId = User.Identity.GetUserId();
            var um = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            try
            {
                var data = um.FindById(userId);
                mv = new SMMVM()
                {

                    platforms = manager.PlatformManager.GetUserPlatforms(data.Level),
                    constraints = manager.PlatformManager.GetPlatformConstraints(),
                    loggedInUser = data
                    //history = manager.LogManager.GetUserHistoryLogs(new User() {Username= data.UserName })
                };
            }
            catch
            {
                mv = new SMMVM()
                {
                    platforms = null,
                    constraints = null,
                    requestHistory = "Unable to load user data, please try again later",
                    loggedInUser = um.FindById(userId)
                };
            }
            return mv;
        }

        // GET: Main/SMM
        public ActionResult SMM()
        {
            SMMVM data;
            try
            {
                data = LoadUserData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> SMM(APIRequest req, string submitButton)
        {
            var data = LoadUserData();
            data.requestData = req;

            if(submitButton == "Start")
            {
                return await Start(req, data);
            }
            else if(submitButton == "Cancel")
            {
                return await Stop(req, data);
            }

            return View(data);
        }

        #region Send/Stop
        /// <summary>
        ///     Both methods should be rewritten for efficiency
        ///     and added into helper methods instead of repeating code
        ///     May, or may not do so once project is finished
        /// </summary>
        private async Task<ActionResult> Stop(APIRequest req, SMMVM data)
        {
            List<API> apis = manager.APIManager.GetAPIs();
            foreach (API api in apis)
            {
                APIRequest request = new APIRequest()
                {
                    URL = api.Link,
                    Handle = req.Handle
                };
                try
                {
                    APIResponse response = await manager.APIManager.SendAPICancelRequest(request);
                    if (response == null)
                    {
                        throw new ApplicationException("Unable to cancel request");
                    }
                    data.requestHistory += $"API Result: {response.result}, API Message: {response.message}\n";
                    await DiscordUtils.LogToDiscord("request", $"{data.loggedInUser.UserName} " +
                        $"has canceled {req.Handle}");
                    try
                    {
                        manager.LogManager.InsertAPIHistory(new APILog()
                        {
                            Link = api.Link,
                            UsedBy = data.loggedInUser.UserName,
                            Response = response.message,
                            Date = DateTime.Now
                        });
                    }
                    catch {}

                    try
                    {
                        manager.LogManager.InsertUserHistory(new UserHistory()
                        {
                            Username = data.loggedInUser.UserName,
                            RequestHandle = req.Handle,
                            RequestType = "Cancel",
                            RequestDetails = "Cancel"
                        });
                    }
                    catch { }

                }
                catch (Exception x)
                {
                    if (x.Message != null && x.InnerException != null)
                    {
                        string errorMsg = x.InnerException.Message.Contains("refused") ?
                            "\nUnable to send stop to APIs, one or all apis are currently offline" :
                            "\nUnable to send stop to APIs, please try again later";

                        data.requestHistory = req.History += errorMsg;
                    }
                    return View(data);
                }
            }
            return View(data);
        }

        private async Task<ActionResult> Start(APIRequest req, SMMVM data)
        {
            if (!req.ToS)
            {
                data.requestHistory = req.History += "\nPlease agree to ToS to continue";
                return View(data);
            }

            string platform = req.Platform.ToLower();
            string fill = req.FillType.ToLower();

            if (fill.Contains("custom"))
            {
                data.requestHistory = req.History += "\nPlease enter a valid fill type";
                return View(data);
            }

            if (!ModelState.IsValid)
            {
                return View(data);
            }
            await DiscordUtils.LogToDiscord("request", $"{data.loggedInUser.UserName} " +
                $"has sent {req.Requests} {req.FillType} to {req.Handle}");
            List<API> apis = manager.APIManager.GetAPIs();
            foreach (API api in apis)
            {
                APIRequest request = new APIRequest()
                {
                    URL = api.Link,
                    Handle = req.Handle,
                    FillType = fill.Split(' ')[1].Split(' ')[0].Replace("[", "").Replace("]", ""),
                    Platform = platform,
                    Requests = req.Requests
                };
                try
                {
                    APIResponse response = await manager.APIManager.SendAPIRequest(request);
                    data.requestHistory += response.message + "\n";

                    try
                    {
                        manager.LogManager.InsertAPIHistory(new APILog()
                        {
                            Link = api.Link,
                            UsedBy = data.loggedInUser.UserName,
                            Response = response.message,
                            Date = DateTime.Now
                        });
                    }
                    catch (Exception ex) { throw ex; }
                    //catch { /* If fails, continue, API would have logging anyways this is redundent */ }

                    try
                    {
                        manager.LogManager.InsertUserHistory(new UserHistory()
                        {
                            Username = data.loggedInUser.UserName,
                            RequestHandle = req.Handle,
                            RequestType = fill,
                            RequestDetails = platform
                        });
                    }
                    catch (Exception ex) { throw ex; }
                }
                catch (Exception x)
                {
                    if(x.Message != null)
                    {
                        string errorMsg = x.InnerException.Message.Contains("refused") ?
                            "\nUnable to send stop to APIs, one or all apis are currently offline" :
                            "\nUnable to send stop to APIs, please try again later";
                        data.requestHistory = req.History += errorMsg;
                    }
                    return View(data);
                }
            }
            return View(data);
        }
        #endregion

        public ActionResult Tools()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateTicket(Guid id, System.Web.Mvc.FormCollection form)
        {
            try
            {
                var manager = MainManager.GetMainManager();
                var userId = User.Identity.GetUserId();
                var um = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = um.FindById(userId);

                if (form["sentMessage"] == null)
                {
                    bool resolved = form["cbResolved"] == "on";
                    string title = form["tbTitle"];
                    int priority = int.Parse(form["selectPriority"]);

                    manager.TicketManager.UpdateTicket(new Ticket()
                    {
                        ID = id,
                        Title = title,
                        Priority = priority,
                        Resolved = resolved
                    });
                }
                else
                {
                    string msg = form["sentMessage"];
                    manager.TicketManager.AddTicketMessage(new TicketMessages()
                    {
                        TicketID = id,
                        Message = msg,
                        Sender = user.UserName,
                        IsStaff = user.Level == 10
                    });
                }

                return RedirectToAction("TicketView", new { id = id });
            }
            catch
            {
                return Error("Unable to update ticket");
            }
        }

        public ActionResult TicketView(Guid? id)
        {
            if (id == null)
            {
                return Error("Invalid GUID");
            }

            try
            {
                Ticket ticket = MainManager.GetMainManager().TicketManager.GetTicket(id.Value);
                if (ticket == null)
                {
                    return Error("Ticket not found");
                }
                return View(ticket);
            }
            catch
            {
                return Error("An error occurred while processing your request");
            }
        }

        public ActionResult Tickets()
        {
            var manager = MainManager.GetMainManager();
            var userId = User.Identity.GetUserId();
            var um = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = um.FindById(userId);

            List<Ticket> tickets = new List<Ticket>();
            try
            {
                if (user.Level == 10)
                {
                    tickets = manager.TicketManager.SelectAllTickets();
                }
                else
                {
                    tickets = manager.TicketManager.SelectUserTickets(user.UserName);
                }
            }
            catch
            {
                return Error("Unable to gather ticket");
            }

            return View(tickets);
        }

        public ActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var manager = MainManager.GetMainManager();
                var userId = User.Identity.GetUserId();
                var um = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = um.FindById(userId);

                try
                {
                    Guid ticketID = Guid.NewGuid();
                    manager.TicketManager.CreateTicket(new Ticket()
                    {
                        ID = ticketID,
                        Title = ticket.Title,
                        Issue = ticket.Issue,
                        Submitter = user.UserName,
                        Priority = ticket.Priority
                    });

                    manager.TicketManager.AddTicketMessage(new TicketMessages()
                    {
                        TicketID = ticketID,
                        Message = ticket.Issue,
                        Sender = user.UserName,
                        IsStaff = false
                    });

                    await DiscordUtils.LogToDiscord("ticket", $"{user.UserName} has created a ticket!" +
                        $"\nID: {ticketID.ToString().Substring(32)}" +
                        $"\nIssue: {ticket.Issue}");

                    return RedirectToAction("Tickets", "Main");
                }
                catch
                {
                    return Error("Unable to create ticket");
                }
            }

            return View(ticket);
        }

        public ActionResult Settings()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception();
                }

                ApplicationUserManager um = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = um.FindById(userId);

                if (user == null)
                {
                    throw new Exception();
                }

                ViewBag.EnableMOTD = manager.UserManager.GetUserSettings(user.UserName).EnableMOTD;
                ViewBag.History = "";
                return View();
            }
            catch (Exception x)
            {
                return Error("Unable to load user settings" + "\n" + x.InnerException.Message);
            }
        }


        [HttpPost]
        public ActionResult Settings(System.Web.Mvc.FormCollection form)
        {
            try
            {
                var motd = form["EnableMOTD"] == "on";
                var btn = form["btnSubmit"];
                var user = form["userName"];

                ViewBag.EnableMOTD = motd;
                ViewBag.History = string.Empty;

                if (btn == "UpdateProfile")
                {
                    var u = new User { Username = user };
                    u.Settings = new UserSettings { EnableMOTD = motd };
                    manager.UserManager.UpdateUserSettings(u);
                }
                else if (btn == "LoadHistory")
                {
                    var u = new User { Username = user };
                    var userHistory = manager.LogManager.GetUserHistoryLogs(u);
                    var history = string.Join("\n", userHistory.Select(item => $"{item.Username}: {item.RequestDetails} - [{item.Date}]"));
                    ViewBag.History = history;
                }
            }
            catch
            {
                return Error("Unable to update settings");
            }

            return View();
        }



        public ActionResult Error(string error)
        {
            ViewBag.ErrorMessage = error;
            return View();
        }
    }
}