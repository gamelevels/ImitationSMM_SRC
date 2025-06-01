using DataInterfaces;
using DataObjects.models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ApplicationAccessor : IApplicationAccessor
    {
        public AppInformation SelectApplicationInformation()
        {
            AppInformation app = null;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_application_information", con, CommandType.StoredProcedure);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows && dbResult.Read())
                    {
                        app = new AppInformation()
                        {
                            MOTD = dbResult.GetString(0),
                            AppVersion = dbResult.GetString(1),
                            TOS = dbResult.GetString(2),
                            Help = dbResult.GetString(3)
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return app;
        }

        public int SelectRegisteredUserCount()
        {
            int count = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_registered_user_count", con, CommandType.StoredProcedure);

                try
                {
                    con.Open();
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return count;
        }
    }
}
