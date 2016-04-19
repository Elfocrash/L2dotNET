using System.Collections.Generic;
using System.Data;
using L2dotNET.Game.db;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.network.loginauth;
using MySql.Data.MySqlClient;
using L2dotNET.Game.network.loginauth.recv;

namespace L2dotNET.Game.network.l2recv
{
    class AuthLogin : GameServerNetworkRequest
    {
        public AuthLogin(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private string _loginName;
        private int _playKey1;
        private int _playKey2;
        private int _loginKey1;
        private int _loginKey2;

        public override void read()
        {
            _loginName = readS();
            _playKey2 = readD();
            _playKey1 = readD();
            _loginKey1 = readD();
            _loginKey2 = readD();
        }

        public override void run()
        {
            LoginSrvTAccount tacc = AuthThread.getInstance().getTA(_loginName.ToLower());

            if (tacc == null)
            {
                getClient().termination();
                return;
            }

            getClient().AccountType = tacc.type;
            getClient().AccountTimeEnd = tacc.timeEnd;
            getClient().AccountTimeLogIn = tacc.timeLogIn;
            getClient().AccountPremium = tacc.premium;
            getClient().AccountPoints = tacc.points;

            getClient().AccountName = _loginName;

            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "SELECT objId FROM user_data WHERE account = '" + _loginName + "' ORDER BY slotId ASC";
            cmd.CommandType = CommandType.Text;

            MySqlDataReader reader = cmd.ExecuteReader();

            List<int> users = new List<int>();
            while (reader.Read())
            {
                users.Add(reader.GetInt32("objId"));
            }

            reader.Close();
            connection.Close();

            int slot = 0;
            foreach (int id in users)
            {
                L2Player p = L2Player.restore(id, getClient());
                p._slotId = slot; slot++;
                Client._accountChars.Add(p);
            }

            getClient().sendPacket(new CharacterSelectionInfo(getClient().AccountName, getClient()._accountChars));
            AuthThread.getInstance().setInGameAccount(getClient().AccountName, true);
        }
    }
}
