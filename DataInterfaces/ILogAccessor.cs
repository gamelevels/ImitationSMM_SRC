using DataObjects.models;
using System.Collections.Generic;

namespace DataInterfaces
{
    public interface ILogAccessor
    {
        List<UserHistory> SelectUserHistoryLogs(User user);
        List<UserLog> SelectUserLogs(User user);
        bool InsertUserLog(UserLog userLog);
        bool InsertUserHistory(UserHistory userHistory);
        bool InsertAPIHistory(APILog apiLog);
    }
}
