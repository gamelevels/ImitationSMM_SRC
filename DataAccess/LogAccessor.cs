using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class LogAccessor : ILogAccessor
    {
        public bool InsertAPIHistory(APILog apiLog)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_insert_api_log", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Link", apiLog.Link);
                cmd.Parameters.AddWithValue("@UsedBy", apiLog.UsedBy);
                cmd.Parameters.AddWithValue("@Response", apiLog.Response);

                try
                {
                    con.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rows == 1;
        }

        public bool InsertUserHistory(UserHistory userHistory)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_insert_user_history", con, CommandType.StoredProcedure);
                cmd.Parameters.AddWithValue("@Username", userHistory.Username);
                cmd.Parameters.AddWithValue("@RequestType", userHistory.RequestType);
                cmd.Parameters.AddWithValue("@RequestHandle", userHistory.RequestHandle);
                cmd.Parameters.AddWithValue("@RequestDetails", userHistory.RequestDetails);

                try
                {
                    con.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rows == 1;
        }

        public bool InsertUserLog(UserLog userLog)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_insert_user_log", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Username", userLog.Username);
                cmd.Parameters.AddWithValue("@LogType", userLog.LogType);
                cmd.Parameters.AddWithValue("@LogDetails", userLog.LogDetails);

                try
                {
                    con.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rows == 1;
        }

        public List<UserHistory> SelectUserHistoryLogs(User user)
        {
            List<UserHistory> userHistory = new List<UserHistory>();
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_user_history", con, CommandType.StoredProcedure);
                cmd.Parameters.AddWithValue("@Username", user.Username);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows)
                    {
                        while (dbResult.Read())
                        {
                            UserHistory uh = new UserHistory()
                            {
                                Username = dbResult.GetString(0),
                                RequestType = dbResult.GetString(1),
                                RequestHandle = dbResult.GetString(2),
                                RequestDetails = dbResult.GetString(3),
                                Date = dbResult.GetDateTime(4)
                            };
                            userHistory.Add(uh);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return userHistory;
        }

        public List<UserLog> SelectUserLogs(User user)
        {
            List<UserLog> userLogs = new List<UserLog>();
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_user_log", con, CommandType.StoredProcedure);
                cmd.Parameters.AddWithValue("@Username", user.Username);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows)
                    {
                        while (dbResult.Read())
                        {
                            UserLog uh = new UserLog()
                            {
                                Username = dbResult.GetString(0),
                                LogType = dbResult.GetString(1),
                                LogDetails = dbResult.GetString(2),
                                Date = dbResult.GetDateTime(3)
                            };
                            userLogs.Add(uh);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return userLogs;
        }
    }
}
