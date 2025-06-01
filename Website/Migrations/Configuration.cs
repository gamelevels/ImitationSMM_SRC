namespace Website.Migrations
{
    using Logic;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Website.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.CodeDom;
    using DataObjects.models;

    internal sealed class Configuration : DbMigrationsConfiguration<Website.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Website.Models.ApplicationDbContext";
        }

        protected override void Seed(Website.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            const string username = "boss";
            Guid newToken = Guid.NewGuid();

            if(!context.Users.Any(u => u.UserName == username))
            {
                MainManager manager = MainManager.GetMainManager();
                manager.UserManager.AddRegisterToken(new RegisterToken()
                {
                    Token = newToken,
                    Days = 999,
                    Level = 10
                });
                var user = new ApplicationUser()
                {
                    UserName = username,
                    Level = 10,
                    ExpirationDate = DateTime.Now.AddYears(30),
                    RegisterToken = newToken,
                    Enabled = true
                };

                IdentityResult resp = um.Create(user, "mypassword");

                User newUser = new User()
                {
                    Username = user.UserName,
                    LevelInfo = new Levels() { Level = user.Level },
                    PasswordHash = manager.UserManager.Hash256("mypassword"),
                    Register = new RegisterToken()
                    {
                        Token = user.RegisterToken
                    },
                    Expiration = user.ExpirationDate
                };

                manager.UserManager.RegisterUser(newUser);
                context.SaveChanges();
            }
        }
    }
}
