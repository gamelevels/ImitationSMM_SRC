using DataObjects.models;
using System;
using System.Collections.Generic;

namespace DataInterfaces
{
    public interface IUserAccessor
    {
        (bool, string) AuthenticateUser(string user, string pass);

        bool RegisterUser(User user);
        UserVM SelectUser(string user);
        bool UpdateUser(User user);
        List<User> SelectUsers();
        int GetUserCooldown(int level);
        RegisterToken GetToken(Guid id);
        Levels GetLevelInformation(int level);
        UserSettings SelectUserSettings(string user);
        bool UpdateUserSettings(User userSettings);
        bool InsertRegisterToken(RegisterToken token);
    }
}