using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class APIAccessor : IAPIAccessor
    {
        public bool AddAPI(API api)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_insert_api", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Link", api.Link);
                cmd.Parameters.AddWithValue("@PlaceholderParameter", api.ParameterPlaceHolder);
                cmd.Parameters.AddWithValue("@Enabled", api.Enabled);

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

        public List<API> GetAllAPI()
        {
            List<API> apis = new List<API>();
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_apis", con, CommandType.StoredProcedure);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows)
                    {
                        while (dbResult.Read())
                        {
                            API api = new API()
                            {
                                Link = dbResult.GetString(0),
                                ParameterPlaceHolder = dbResult.GetString(1),
                                Enabled = dbResult.GetBoolean(2)
                            };
                            apis.Add(api);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return apis;
        }

        public bool RemoveAPI(API api)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAPI(API api)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_update_api", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Link", api.Link);
                cmd.Parameters.AddWithValue("@ParameterPlaceHolder", api.ParameterPlaceHolder);
                cmd.Parameters.AddWithValue("@Enabled", api.Enabled);

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
    }
}
