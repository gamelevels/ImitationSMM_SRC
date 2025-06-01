using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Website.Models
{
    // Custom authorization class to work with users levels
    // Instead of implementing roles and reworking functionality

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LevelAuthorization : FilterAttribute, IAuthorizationFilter
    {
        private int _level;

        public LevelAuthorization(int level)
        {
            _level = level;
        }

        // Send to login if not logged in, send to home if not required level
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var applicationUser = userManager.FindById(user.Identity.GetUserId()); 
            if (applicationUser == null || applicationUser.Level != _level)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
        }
    }
}