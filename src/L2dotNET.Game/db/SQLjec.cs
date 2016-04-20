using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Contexts;
using L2dotNET.Game.logger;
using MySql.Data.MySqlClient;

namespace L2dotNET.Game.db
{
    [Synchronization]
    public class SQLjec
    {
        private static SQLjec sql = new SQLjec();
        public static SQLjec getInstance()
        {
            return sql;
        }

        protected MySqlConnectionStringBuilder connBuilder;

        public SQLjec()
        {
            connBuilder = new MySqlConnectionStringBuilder();
            connBuilder.Database = Cfg.DB_NAME;
            connBuilder.Server = Cfg.DB_HOST;
            connBuilder.UserID = Cfg.DB_USER;
            connBuilder.Password = Cfg.DB_PASS;
            connBuilder.Port = 3306;
           // connection = new MySqlConnection(connBuilder.ConnectionString);

            CLogger.info("MySQL:  established");
        }

        public MySqlConnection conn()
        {
            return new MySqlConnection(connBuilder.ConnectionString);
        }
    }

    public class SQL_Block
    {
        private List<string[]> values = new List<string[]>();
        private List<string[]> values2 = new List<string[]>();
        private string _table;
        public SQL_Block(string table)
        {
            _table = table;
        }

        public void param(string p, object v)
        {
            values.Add(new string[] { p, v.ToString() });
        }

        public void where(string p, object v)
        {
            values2.Add(new string[] { p, v.ToString() });
        }

        MySqlConnection connection;
        MySqlCommand cmd;
        public void on()
        {
            connection = SQLjec.getInstance().conn();
            cmd = connection.CreateCommand();
            connection.Open();
        }

        public void off()
        {
            connection.Close();
        }

        public void sql_insert(bool ex)
        {
            if (!ex)
            {
                connection = SQLjec.getInstance().conn();
                cmd = connection.CreateCommand();
                connection.Open();
            }

            string pars = "", vals = "";
            short x = 0;
            foreach (string[] d in values)
            {
                pars += d[0];
                vals += "'"+d[1] + "'";

                x++;
                if (x < values.Count)
                {
                    pars += ",";
                    vals += ",";
                }
            }

            cmd.CommandText = "insert into "+_table+" ("+pars+") values ("+vals+")";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            if (!ex)
                connection.Close();
            else
            {
                values.Clear();
                values2.Clear();
            }
        }

        public void sql_delete(bool ex)
        {
            if (!ex)
            {
                connection = SQLjec.getInstance().conn();
                cmd = connection.CreateCommand();
                connection.Open();
            }

            string where = ""; 
            short x = 0;
            foreach (string[] d in values2)
            {
                where += d[0] + "='" + d[1] + "'";

                x++;
                if (x < values2.Count)
                {
                    where += " and ";
                }
            }

            cmd.CommandText = "delete from " + _table + " where " + where + "";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            if(!ex)
                connection.Close();
            else
            {
                values.Clear();
                values2.Clear();
            }
        }

        public void sql_update(bool ex)
        {
            if (!ex)
            {
                connection = SQLjec.getInstance().conn();
                cmd = connection.CreateCommand();
                connection.Open();
            }

            string str = "", where = "";
            byte x = 0;
            foreach (string[] d in values)
            {
                str += d[0] + "='" + d[1] + "'";

                x++;
                if (x < values.Count)
                {
                    str += ",";
                }
            }

            x = 0;
            foreach (string[] d in values2)
            {
                where += d[0] + "='" + d[1] + "'";

                x++;
                if (x < values2.Count)
                {
                    where += " and ";
                }
            }

            //cmd.CommandText = "update " + _table + " set " + str + " where " + where;
            //cmd.CommandType = CommandType.Text;
            //cmd.ExecuteNonQuery();

            if (!ex)
                connection.Close();
            else
            {
                values.Clear();
                values2.Clear();
            }
        }
    }
}
