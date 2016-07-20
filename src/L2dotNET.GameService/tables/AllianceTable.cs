using System.Collections.Generic;
using L2dotNET.GameService.Model.Communities;

namespace L2dotNET.GameService.Tables
{
    class AllianceTable
    {
        private static volatile AllianceTable _instance;
        private static readonly object SyncRoot = new object();

        public static AllianceTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AllianceTable();
                }

                return _instance;
            }
        }

        public void Initialize()
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

            //CLogger.info($"Community: loaded {_alliances.Count} alliances.");
        }

        public SortedList<int, L2Alliance> Alliances = new SortedList<int, L2Alliance>();

        public L2Alliance GetAlliance(int id)
        {
            return Alliances.ContainsKey(id) ? Alliances[id] : null;
        }
    }
}