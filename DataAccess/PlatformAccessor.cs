using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class PlatformAccessor : IPlatformAccessor
    {
        public List<Platform> SelectAllPlatforms()
        {
            List<Platform> platforms = new List<Platform>();
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_platforms", con, CommandType.StoredProcedure);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows)
                    {
                        while (dbResult.Read())
                        {
                            Platform plat = new Platform()
                            {
                                PlatformName = dbResult.GetString(0),
                                MinimumLevel = dbResult.GetByte(1),
                                Enabled = dbResult.GetBoolean(2)
                            };
                            platforms.Add(plat);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return platforms;
        }

        public List<PlatformConstraint> SelectPlatformConstraints()
        {
            List<PlatformConstraint> platforms = new List<PlatformConstraint>();
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_platform_constraints", con, CommandType.StoredProcedure);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows)
                    {
                        while (dbResult.Read())
                        {
                            PlatformConstraint plat = new PlatformConstraint()
                            {
                                PlatformName = dbResult.GetString(0),
                                RequestCount = dbResult.GetInt16(1),
                                RequestType = dbResult.GetString(2)
                            };

                            platforms.Add(plat);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return platforms;
        }

        public Dictionary<string, int> SelectPopularPlatform()
        {
            Dictionary<string, int> plats = new Dictionary<string, int>();
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_popular_platform", con, CommandType.StoredProcedure);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows)
                    {
                        while (dbResult.Read())
                        {
                            plats.Add(dbResult.GetString(0), dbResult.GetInt32(1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return plats;
        }
    }
}
