using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace L2dotNET.Auth.data
{
    class SQLjec
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
            connBuilder.Add("Database", Cfg.DB_NAME);
            connBuilder.Add("Data Source", Cfg.DB_HOST);
            connBuilder.Add("User Id", Cfg.DB_USER);
            connBuilder.Add("Password", Cfg.DB_PASS);
          
            Console.WriteLine("SQL connection established");
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

        public void sql_insert()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();
            connection.Open();

            string pars = "", vals = "";
            short x = 0;
            foreach (string[] d in values)
            {
                pars += d[0];
                vals += "'" + d[1] + "'";

                x++;
                if (x < values.Count)
                {
                    pars += ",";
                    vals += ",";
                }
            }

            cmd.CommandText = "insert into " + _table + " (" + pars + ") values (" + vals + ")";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public void sql_delete()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();
            connection.Open();

            string str = "";
            short x = 0;
            foreach (string[] d in values)
            {
                str += d[0] + "='" + d[1] + "'";

                x++;
                if (x < values.Count)
                {
                    str += " and ";
                }
            }

            cmd.CommandText = "delete from " + _table + " where " + str + "";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public void sql_update()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();
            connection.Open();

            string str = "", where = "";
            short x = 0;
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
                    str += " and ";
                }
            }

            cmd.CommandText = "update " + _table + " set " + str + " where " + where;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
