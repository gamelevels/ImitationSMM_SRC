using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public static class SqlUtils
    {
        public static SqlConnection NewConnection()
        {
            return new SqlConnection(@"Data Source=localhost;Initial Catalog=Imitation;Integrated Security=True");
        }

        public static SqlCommand NewCommand(string txt, SqlConnection con, CommandType type)
        {
            SqlCommand myCmd = new SqlCommand(txt, con);
            myCmd.CommandType = type;
            return myCmd;
        }
    }
}