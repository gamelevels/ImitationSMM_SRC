using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFakes
{
    public class UserAccessFake : IUserAccessor
    {
        private List<UserVM> users = new List<UserVM>();
        private List<Levels> levels = new List<Levels>();
        private List<RegisterToken> registerTokens = new List<RegisterToken>();
        private List<Platform> platforms = new List<Platform>();
        public UserAccessFake()
        {
            users.Add(new UserVM()
            {
                Username = "Test",
                LevelInfo = new Levels() { Level = 1 },
                Expiration = DateTime.Parse("12/12/2030"),
                Register = new RegisterToken()
                {
                    Token = new Guid("333702f9-859a-4dac-92b0-638999a666a8")
                },
                Enabled = false,
                PasswordHash = "89e01536ac207279409d4de1e5253e01f4a1769e696db0d6062ca9b8f56767c8",
                Settings = new UserSettings()
                {
                    EnableMOTD = true
                }
            });
            users.Add(new UserVM()
            {
                Username = "Test1",
                LevelInfo = new Levels() { Level = 2 },
                Expiration = DateTime.Now - TimeSpan.FromDays(1),
                Register = new RegisterToken()
                {
                    Token = new Guid("333702f9-859a-4dac-92b0-638999a666a8")
                },
                Enabled = true,
                PasswordHash = "89e01536ac207279409d4de1e5253e01f4a1769e696db0d6062ca9b8f56767c8",
                Settings = new UserSettings()
                {
                    EnableMOTD = true
                }
            });
            users.Add(new UserVM()
            {
                Username = "Test2",
                LevelInfo = new Levels() { Level = 3 },
                Expiration = DateTime.Parse("12/12/2030"),
                Register = new RegisterToken()
                {
                    Token = new Guid("333702f9-859a-4dac-92b0-638999a666a8")
                },
                Enabled = true,
                PasswordHash = "89e01536ac207279409d4de1e5253e01f4a1769e696db0d6062ca9b8f56767c8",
                Settings = new UserSettings()
                {
                    EnableMOTD = true
                }
            });
            platforms.Add(new Platform() { Enabled = true, MinimumLevel = 1, PlatformName = "Tiktok" });
            platforms.Add(new Platform() { Enabled = true, MinimumLevel = 2, PlatformName = "Facebook" });
            platforms.Add(new Platform() { Enabled = true, MinimumLevel = 3, PlatformName = "Youtube" });
            levels.Add(new Levels() { Level = 1, HandleCooldown = 60 });
            levels.Add(new Levels() { Level = 2, HandleCooldown = 30 });
            levels.Add(new Levels() { Level = 3, HandleCooldown = 15 });
            registerTokens.Add(new RegisterToken() { Token = new Guid("333702f9-859a-4dac-92b0-638999a666a8") });
            registerTokens.Add(new RegisterToken() { Token = new Guid("333702f9-859a-4dac-92b0-638999a666a9") });

        }
        public (bool, string) AuthenticateUser(string user, string passw)
        {
            bool result = false;
            string resultMsg = "";

            UserVM myUser = null;
            myUser = users.FirstOrDefault(u => u.Username == user && u.PasswordHash == passw);
            if (myUser != null)
            {
                if (myUser.Expiration < DateTime.Now)
                {
                    resultMsg = "Account is expired";
                }
                else if (!myUser.Enabled)
                {
                    resultMsg = "Account is disabled";
                }
                else
                {
                    result = true;
                    resultMsg = "Authenticated";
                }
            }
            else
            {
                resultMsg = "Invalid username or password";
            }

            return (result, resultMsg);
        }

        public UserVM LoginUser(string username, string password)
        {
            // skipped calling this
            // just using authenticate user
            return new UserVM();
        }

        public List<Platform> GetUserPlatforms(int level)
        {
            return platforms.FindAll(w => w.MinimumLevel <= level);
        }

        public bool RegisterUser(User user)
        {
            // too much logic to implement here
            // just gonna skip to save time
            // actual logic already works
            throw new NotImplementedException();
        }

        public UserVM GetUser(string user)
        {
            return users.First(u => u.Username == user);
        }

        public UserVM SelectUser(string user)
        {
            // does same as above, changed logic
            throw new NotImplementedException();
        }

        public int GetUserCooldown(int level)
        {
            return levels.First(u => u.Level == level).HandleCooldown;
        }

        public RegisterToken GetToken(Guid id)
        {
            return registerTokens.First(t => t.Token == id);
        }

        public Levels GetLevelInformation(int level)
        {
            return levels.First(l => l.Level == level);
        }

        public UserSettings SelectUserSettings(string user)
        {
            return users.First(u => u.Username == user).Settings;
        }

        public bool UpdateUserSettings(User user)
        {
            bool result = false;
            try
            {
                User u = users.First(x => x.Username == user.Username);
                u.Settings = user.Settings;
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;

        }

        public List<User> SelectUsers()
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool InsertRegisterToken(RegisterToken token)
        {
            throw new NotImplementedException();
        }
    }
}