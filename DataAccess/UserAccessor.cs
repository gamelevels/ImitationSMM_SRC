using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInterfaces;
using DataObjects.models;
using System.Diagnostics;
using System.Reflection.Emit;

namespace DataAccess
{
    public class UserAccessor : IUserAccessor
    {
        public (bool, string) AuthenticateUser(string user, string pass)
        {
            bool result = false;
            string resultMsg = "";

            using(var con = SqlUtils.NewConnection()) 
            using(SqlCommand cmd = SqlUtils.NewCommand("sp_authenticate_user", con, CommandType.StoredProcedure))
            {
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 64).Value = user;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.Char, 64).Value = pass;

                try
                {
                    con.Open();
                    int dbResult = Convert.ToInt32(cmd.ExecuteScalar());

                    // since this is only c# 7.3 cant use a recursive switch statement
                    switch(dbResult)
                    {
                        case 1:
                            result = true;
                            resultMsg = "Authenticated";
                            break;
                        case 2:
                        case 3:
                            resultMsg = "Invalid username or password";
                            break;
                        case 4:
                            resultMsg = "Account is disabled";
                            break;
                        case 5:
                            resultMsg = "Account is expired";
                            break;
                        default:
                            resultMsg = "Unable to login";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return (result, resultMsg);
        }

        public Levels GetLevelInformation(int level)
        {
            Levels lvl = null;
            using (var con = SqlUtils.NewConnection())
            using (SqlCommand cmd = SqlUtils.NewCommand("sp_select_level_information", con, CommandType.StoredProcedure))
            {
                cmd.Parameters.AddWithValue("@Level", level);

                try
                {
                    con.Open();
                    using (SqlDataReader dbResult = cmd.ExecuteReader())
                    {
                        while(dbResult.Read())
                        {
                            lvl = new Levels()
                            {
                                Level = level,
                                HandleCooldown = dbResult.GetByte(1)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return lvl;
        }

        public RegisterToken GetToken(Guid id)
        {
            RegisterToken regToken = null;
            using(var con = SqlUtils.NewConnection())
            using(SqlCommand cmd = SqlUtils.NewCommand("sp_get_register_token", con, CommandType.StoredProcedure))
            {
                cmd.Parameters.Add("@Token", SqlDbType.UniqueIdentifier).Value = id;

                try
                {
                    con.Open();
                    using(SqlDataReader dbResult = cmd.ExecuteReader())
                    {
                        while (dbResult.Read())
                        {
                            regToken = new RegisterToken()
                            {
                                Token = dbResult.GetGuid(0),
                                Level = dbResult.GetInt16(1),
                                Days = dbResult.GetInt16(2)
                            };
                            regToken.UsedBy = dbResult["UsedBy"] != DBNull.Value ? dbResult.GetString(3) : null;
                            regToken.UsedDate = dbResult["UsedDate"] != DBNull.Value ? dbResult.GetDateTime(4) : DateTime.Now;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return regToken;
        }

        public int GetUserCooldown(int level)
        {
            int cooldown = 0;
            using (var con = SqlUtils.NewConnection())
            using(SqlCommand cmd = SqlUtils.NewCommand("sp_select_level_information", con, CommandType.StoredProcedure))
            {
                cmd.Parameters.Add("@Level", SqlDbType.SmallInt).Value = level;
                try
                {
                    con.Open();
                    cooldown = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return cooldown;
        }

        public bool RegisterUser(User user)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_insert_new_user", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@Level", user.LevelInfo.Level);
                cmd.Parameters.AddWithValue("@Expiration", user.Expiration);
                cmd.Parameters.Add("@Token", SqlDbType.UniqueIdentifier).Value = user.Register.Token;

                try
                {
                    con.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                cmd = SqlUtils.NewCommand("sp_claim_register_token", con, CommandType.StoredProcedure);
                cmd.Parameters.AddWithValue("@UsedBy", user.Username);
                cmd.Parameters.Add("@Token", SqlDbType.UniqueIdentifier).Value = user.Register.Token;

                try
                {
                    rows += cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                cmd = SqlUtils.NewCommand("sp_insert_user_settings", con, CommandType.StoredProcedure);
                cmd.Parameters.AddWithValue("@Username", user.Username);

                try
                {
                    rows += cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return rows == 3;
        }

        public UserVM SelectUser(string user)
        {
            UserVM userVM = null;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_user_information", con, CommandType.StoredProcedure);
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 64);
                cmd.Parameters["@Username"].Value = user;

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if(dbResult.HasRows && dbResult.Read())
                    {
                        userVM = new UserVM()
                        {
                            Username = user,
                            LevelInfo = new Levels()
                            {
                                Level = dbResult.GetByte(1)
                            },
                            Expiration = dbResult.GetDateTime(2),
                            Register = new RegisterToken()
                            {
                                Token = dbResult.GetGuid(3)
                            },
                            Enabled = dbResult.GetBoolean(4)
                        };
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return userVM;
        }

        public List<User> SelectUsers()
        {
            List<User> users = new List<User>();
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_all_users", con, CommandType.StoredProcedure);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows)
                    {
                        while (dbResult.Read())
                        {
                            User user = new User()
                            {
                                Username = dbResult.GetString(0),
                                LevelInfo = new Levels()
                                {
                                    Level = dbResult.GetByte(1)
                                },
                                Expiration = dbResult.GetDateTime(2),
                                Register = new RegisterToken()
                                {
                                    Token = dbResult.GetGuid(3)
                                },
                                Enabled = dbResult.GetBoolean(4)
                            };
                            users.Add(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return users;
        }

        public UserSettings SelectUserSettings(string user)
        {
            UserSettings settings = null;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_select_user_settings", con, CommandType.StoredProcedure);
                cmd.Parameters.AddWithValue("@Username", user);

                try
                {
                    con.Open();
                    SqlDataReader dbResult = cmd.ExecuteReader();
                    if (dbResult.HasRows && dbResult.Read())
                    {
                        settings = new UserSettings()
                        {
                            EnableMOTD = dbResult.GetBoolean(0)
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return settings;
        }

        public bool UpdateUser(User user)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_update_user", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Level", user.LevelInfo.Level);
                cmd.Parameters.AddWithValue("@Expiration", user.Expiration);
                cmd.Parameters.AddWithValue("@Enabled", user.Enabled);

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

        public bool UpdateUserSettings(User user)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_update_user_settings", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@EnableMOTD", user.Settings.EnableMOTD);

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

        public bool InsertRegisterToken(RegisterToken token)
        {
            int rows = 0;
            using (var con = SqlUtils.NewConnection())
            {
                SqlCommand cmd = SqlUtils.NewCommand("sp_insert_register_token", con, CommandType.StoredProcedure);

                cmd.Parameters.AddWithValue("@Token", token.Token);
                cmd.Parameters.AddWithValue("@Level", token.Level);
                cmd.Parameters.AddWithValue("@Days", token.Days);

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
