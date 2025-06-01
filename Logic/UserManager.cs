using DataAccess;
using DataInterfaces;
using DataObjects;
using DataObjects.models;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

namespace Logic
{
    public class UserManager : IUserManager
    {
        private IUserAccessor userAccessor = null;

        public UserManager()
        {
            userAccessor = new UserAccessor();
        }

        public UserManager(IUserAccessor access)
        {
            userAccessor = access;
        }

        public (bool, string) AuthenticateUser(string username, string password)
        {
            try
            {
                return userAccessor.AuthenticateUser(username, Hash256(password));
            }
            catch
            { 
                throw new ApplicationException("Invalid username or password");
            }
        }

        public UserVM FindUser(string username)
        {
            try
            {
                return userAccessor.SelectUser(username);
            }
            catch
            {
                throw new ApplicationException("Unable to find user");
            }
        }


        public Levels GetLevelInformation(int level)
        {
            try
            {
                return userAccessor.GetLevelInformation(level);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find level information", ex);
            }
        }

        public RegisterToken GetToken(Guid id)
        {
            try
            {
                return userAccessor.GetToken(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find token", ex);
            }
        }

        public UserVM GetUser(string username)
        {
            try
            {
                UserVM user = userAccessor.SelectUser(username);
                user.LevelInfo = userAccessor.GetLevelInformation(user.LevelInfo.Level);
                user.Register = userAccessor.GetToken(user.Register.Token);
                user.Settings = userAccessor.SelectUserSettings(user.Username);
                return user;
            }
            catch
            {
                throw new ApplicationException("Unable to find user");
            }
        }

        public List<User> GetUsers()
        {
            try
            {
                // Might refactor
                List<User> users = userAccessor.SelectUsers();
                foreach(User user in users) {
                    bool motd = userAccessor.SelectUserSettings(user.Username).EnableMOTD;
                    user.Settings = new UserSettings() 
                    { 
                        EnableMOTD = motd
                    };
                }
                return users;
            }
            catch
            {
                throw new ApplicationException("Unable to find users");
            }
        }

        public UserSettings GetUserSettings(string user)
        {
            try
            {
                return userAccessor.SelectUserSettings(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Unable to load user settings: {ex.Message}");
            }
        }

        public string Hash256(string input)
        {
            byte[] data = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", data.Select(b => b.ToString("x2")));
        }

        public UserVM LoginUser(string username, string password)
        {

            try
            {
                // Might refactor into authentication response
                // However we're also using entity so maybe not.
                var result = AuthenticateUser(username, password);
                if (result.Item1)
                {
                    return GetUser(username);
                }
                else
                {
                    throw new ArgumentException(result.Item2);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Authentication Failed: {ex.Message}");
            }
        }

        public bool RegisterUser(User user)
        {
            try
            {
                if (userAccessor.RegisterUser(user))
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("Unable to register user, try again later");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Registration Failed: {ex.Message}");
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                return userAccessor.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update user", ex);
            }
        }

        public bool UpdateUserSettings(User settings)
        {
            try
            {
                return userAccessor.UpdateUserSettings(settings);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update settings", ex);
            }
        }

        public bool AddRegisterToken(RegisterToken token)
        {
            try
            {
                return userAccessor.InsertRegisterToken(token);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to add register token", ex);
            }
        }

        /// May implement this later,
        /// however I figured it wouldn't be as easily readable
        //private bool UpdateEntity<T>(T model, Func<T, bool> method, string error)
        //{
        //    try
        //    {
        //        return method(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException(error, ex);
        //    }
        //}
    }
}