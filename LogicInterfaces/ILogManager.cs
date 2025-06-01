using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface ILogManager
    {

        List<UserHistory> GetUserHistoryLogs(User user);
        List<UserLog> GetUserLogs(User user);
        bool InsertUserLog(UserLog userLog);
        bool InsertUserHistory(UserHistory userHistory);
        bool InsertAPIHistory(APILog apiLog);
    }
}
