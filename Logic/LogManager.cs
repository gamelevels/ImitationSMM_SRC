using DataAccess;
using DataInterfaces;
using DataObjects.models;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class LogManager : ILogManager
    {
        private ILogAccessor logAccessor = null;

        public LogManager()
        {
            logAccessor = new LogAccessor();
        }

        public LogManager(ILogAccessor access)
        {
            logAccessor = access;
        }

        public List<UserHistory> GetUserHistoryLogs(User user)
        {
            try
            {
                return logAccessor.SelectUserHistoryLogs(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find user hisotry", ex);
            }
        }

        public List<UserLog> GetUserLogs(User user)
        {
            try
            {
                return logAccessor.SelectUserLogs(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find user logs", ex);
            }
        }

        public bool InsertAPIHistory(APILog apiLog)
        {
            try
            {
                return logAccessor.InsertAPIHistory(apiLog);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to insert api log", ex);
            }
        }

        public bool InsertUserHistory(UserHistory userHistory)
        {
            try
            {
                return logAccessor.InsertUserHistory(userHistory);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to insert user history", ex);
            }
        }

        public bool InsertUserLog(UserLog userLog)
        {
            try
            {
                return logAccessor.InsertUserLog(userLog);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to insert user log", ex);
            }
        }
    }
}
