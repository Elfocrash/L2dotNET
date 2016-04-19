using System;
using System.Data;
using L2dotNET.Game.db;
using MySql.Data.MySqlClient;

namespace L2dotNET.Game.tables
{
    class IdFactory
    {
        private static IdFactory idf = new IdFactory();

        public static IdFactory getInstance()
        {
            return idf;
        }

        public int id_min = 1, id_max = 0x7FFFFFFF;
        protected int currentId = 1;

        public int nextId()
        {
            currentId++;
            return currentId;
        }

        public void init()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "SELECT objId FROM user_data";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int dbnum = reader.GetInt32("objId");

                if (dbnum >= currentId)
                    currentId = dbnum++;
            }
            reader.Close();


            cmd.CommandText = "SELECT iobjectId FROM user_items";
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int dbnum = reader.GetInt32("iobjectId");

                if (dbnum >= currentId)
                    currentId = dbnum++;
            }
            reader.Close();

            connection.Close();

            Console.WriteLine("idfactory: used ids " + currentId);
        }
    }
}
