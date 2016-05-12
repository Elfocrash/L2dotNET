using System.Collections.Generic;
using System.Data;
using L2dotNET.GameService.model.communities;
using MySql.Data.MySqlClient;

namespace L2dotNET.GameService.tables
{
    class AllianceTable
    {
        private static AllianceTable instance = new AllianceTable();
        public static AllianceTable getInstance()
        {
            return instance;
        }

        public SortedList<int, L2Alliance> _alliances = new SortedList<int, L2Alliance>();

        public AllianceTable()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM alliance_data";
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    L2Alliance ally = new L2Alliance();
            //    ally.ID = reader.GetInt32("id");
            //    ally.Name = reader.GetString("name");
            //    ally.CrestID = reader.GetInt32("crestId");
            //    ally.LeaderID = reader.GetInt32("leaderId");

            //    _alliances.Add(ally.ID, ally);
            //}

            //reader.Close();
            //connection.Close();

            //CLogger.info("Community: loaded "+_alliances.Count+" alliances.");
        }

        public L2Alliance getAlliance(int id)
        {
            if (_alliances.ContainsKey(id))
                return _alliances[id];
            return null;
        }
    }
}
