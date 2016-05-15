using System.Collections.Generic;
using System.Data;
using System.IO;
using L2dotNET.GameService.model.communities;
using MySql.Data.MySqlClient;

namespace L2dotNET.GameService.tables
{
    class ClanTable
    {
        private static ClanTable instance = new ClanTable();
        public static ClanTable getInstance()
        {
            return instance;
        }

        public SortedList<int, L2Clan> _clans = new SortedList<int, L2Clan>();

        public ClanTable()
        {
            //MySqlConnection connection = SQLjec.getInstance().conn();
            //MySqlCommand cmd = connection.CreateCommand();

            //connection.Open();

            //cmd.CommandText = "SELECT * FROM clan_data";
            //cmd.CommandType = CommandType.Text;

            //MySqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    L2Clan clan = new L2Clan();
            //    clan.ClanID = reader.GetInt32("id");
            //    clan.Name = reader.GetString("name");
            //    clan.Level = (byte)reader.GetInt16("level");
            //    clan.LeaderID = reader.GetInt32("leaderId");
            //    clan.ClanMasterName = reader.GetString("leaderName");
            //    clan.CrestID = reader.GetInt32("crestId");
            //    if (clan.CrestID > 0)
            //    {
            //        try
            //        {
            //            FileStream fs = new FileStream(@"crests\c" + clan.CrestID + ".bmp", FileMode.Open, FileAccess.Read);
            //            BinaryReader br = new BinaryReader(fs);
            //            long numBytes = new FileInfo(@"crests\c" + clan.CrestID + ".bmp").Length;
            //            clan.CrestPicture = br.ReadBytes((int)numBytes);
            //        }
            //        catch
            //        {
            //            CLogger.warning("failed to load clan crest.");
            //        }
            //    }
            //    clan.LargeCrestID = reader.GetInt32("crestBigId");
            //    if (clan.LargeCrestID > 0)
            //    {
            //        try
            //        {
            //            FileStream fs = new FileStream(@"crests\b" + clan.LargeCrestID + ".bmp", FileMode.Open, FileAccess.Read);
            //            BinaryReader br = new BinaryReader(fs);
            //            long numBytes = new FileInfo(@"crests\b" + clan.LargeCrestID + ".bmp").Length;
            //            clan.CrestLargePicture = br.ReadBytes((int)numBytes);
            //        }
            //        catch
            //        {
            //            CLogger.warning("failed to load clan crest.");
            //        }
            //    }
            //    clan.CastleID = reader.GetInt32("castleId");
            //    clan.HideoutID = reader.GetInt32("agitId");

            //    if (clan.HideoutID > 0)
            //    {
            //        clan.hideout = StructureTable.getInstance().hideouts[clan.HideoutID];
            //        clan.hideout.ownerId = clan.ClanID;
            //    }

            //    clan.FortressID = reader.GetInt32("fortId");
            //    clan.JoinDominionWarID = reader.GetInt32("dominionId");
            //    clan.AllianceID = reader.GetInt32("allyId");

            //    if (clan.AllianceID > 0)
            //    {
            //        L2Alliance alliance = AllianceTable.getInstance().getAlliance(clan.AllianceID);
            //        if (alliance != null)
            //        {
            //            clan.Alliance = alliance;
            //        }
            //        else
            //            clan.AllianceID = 0;
            //    }

            //    SQLjec sq = new SQLjec();
            //    MySqlConnection conn2 = sq.conn();
            //    MySqlCommand msc_1 = conn2.CreateCommand();
            //    conn2.Open();
            //    msc_1.CommandText = "SELECT * FROM characters where clanId=" + clan.ClanID;
            //    msc_1.CommandType = CommandType.Text;

            //    MySqlDataReader members_r = msc_1.ExecuteReader();

            //    while (members_r.Read())
            //    {
            //        ClanMember cm = new ClanMember();
            //        cm.classId = (byte)members_r.GetInt16("pActiveClass");
            //        cm.Level = (byte)members_r.GetInt16("plevel");
            //        cm.Name = members_r.GetString("pname");
            //        cm.ObjID = members_r.GetInt32("objId");
            //        cm.sponsorId = 0;
            //        cm.NickName = members_r.GetString("ptitle");
            //        cm.ClanType = members_r.GetInt16("clanType");
            //        cm.ClanPrivs = members_r.GetInt32("clanPrivs");
            //        switch (cm.ClanType)
            //        {
            //            case 0:
            //                cm._pledgeTypeName = clan.Name;
            //                break;
            //        }

            //        clan.members.Add(cm);
            //    }

            //    members_r.Close();

            //    MySqlCommand msc_2 = conn2.CreateCommand();
            //    msc_2.CommandText = "SELECT * FROM clan_sub where id=" + clan.ClanID;
            //    msc_2.CommandType = CommandType.Text;

            //    MySqlDataReader subs = msc_2.ExecuteReader();
            //    while (subs.Read())
            //    {
            //        short type = subs.GetInt16("subId");
            //        string name = subs.GetString("name");
            //        int leaderId = subs.GetInt32("leaderId");
            //        string leaderName = subs.GetString("leaderName");
            //        switch (type)
            //        {
            //            case (short)e_ClanType.CLAN_ACADEMY:
            //                clan.Academy = new e_ClanSub(e_ClanType.CLAN_ACADEMY);
            //                clan.Academy.LeaderID = clan.LeaderID;
            //                clan.Academy.Name = name;
            //                break;
            //            case (short)e_ClanType.CLAN_KNIGHT1:
            //                clan.Knights_1 = new e_ClanSub(e_ClanType.CLAN_KNIGHT1);
            //                clan.Knights_1.LeaderID = leaderId;
            //                clan.Knights_1.LeaderName = leaderName;
            //                clan.Knights_1.Name = name;
            //                break;
            //            case (short)e_ClanType.CLAN_KNIGHT2:
            //                clan.Knights_2 = new e_ClanSub(e_ClanType.CLAN_KNIGHT2);
            //                clan.Knights_2.LeaderID = leaderId;
            //                clan.Knights_2.LeaderName = leaderName;
            //                clan.Knights_2.Name = name;
            //                break;
            //            case (short)e_ClanType.CLAN_KNIGHT3:
            //                clan.Knights_1_Order1 = new e_ClanSub(e_ClanType.CLAN_KNIGHT3);
            //                clan.Knights_1_Order1.LeaderID = leaderId;
            //                clan.Knights_1_Order1.LeaderName = leaderName;
            //                clan.Knights_1_Order1.Name = name;
            //                break;
            //            case (short)e_ClanType.CLAN_KNIGHT4:
            //                clan.Knights_1_Order2 = new e_ClanSub(e_ClanType.CLAN_KNIGHT4);
            //                clan.Knights_1_Order2.LeaderID = leaderId;
            //                clan.Knights_1_Order2.LeaderName = leaderName;
            //                clan.Knights_1_Order2.Name = name;
            //                break;
            //            case (short)e_ClanType.CLAN_KNIGHT5:
            //                clan.Knights_2_Order1 = new e_ClanSub(e_ClanType.CLAN_KNIGHT5);
            //                clan.Knights_2_Order1.LeaderID = leaderId;
            //                clan.Knights_2_Order1.LeaderName = leaderName;
            //                clan.Knights_2_Order1.Name = name;
            //                break;
            //            case (short)e_ClanType.CLAN_KNIGHT6:
            //                clan.Knights_2_Order2 = new e_ClanSub(e_ClanType.CLAN_KNIGHT6);
            //                clan.Knights_2_Order2.LeaderID = leaderId;
            //                clan.Knights_2_Order2.LeaderName = leaderName;
            //                clan.Knights_2_Order2.Name = name;
            //                break;
            //        }
            //    }

            //    conn2.Close();
            //    _clans.Add(clan.ClanID, clan);
            //}

            //reader.Close();
            //connection.Close();

            //CLogger.info("Community: loaded " + _clans.Count + " clans.");
        }

        public void apply(L2Player player)
        {
            if (!_clans.ContainsKey(player.ClanId))
            {
                player.ClanId = 0;
                return;
            }

            _clans[player.ClanId].onEnter(player);
        }

        public L2Clan getClan(int id)
        {
            if (!_clans.ContainsKey(id))
                return null;
            return _clans[id];
        }
    }
}
