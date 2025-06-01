using DataObjects;
using DataObjects.models;
using System;
using System.Collections.Generic;

namespace LogicInterfaces
{
    public interface IUserManager
    {
        UserVM LoginUser(string username, string password);
        string Hash256(string data);
        (bool, string) AuthenticateUser(string username, string password);
        UserVM GetUser(string username);
        UserVM FindUser(string username);
        bool UpdateUser(User user);
        List<User> GetUsers();
        bool RegisterUser(User user);
        bool AddRegisterToken(RegisterToken token);

        #region Regsiter Tokens
        RegisterToken GetToken(Guid id);
        #endregion

        #region Level
        Levels GetLevelInformation(int level);
        #endregion

        #region UserSettings
        UserSettings GetUserSettings(string user);
        bool UpdateUserSettings(User settings);
        #endregion
    }
}