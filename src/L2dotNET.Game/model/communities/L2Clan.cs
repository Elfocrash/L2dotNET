using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using L2dotNET.GameService.model.structures;
using L2dotNET.GameService.network;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.communities
{
    public class L2Clan
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2Clan));

        public byte Level = 0;
        public int LeaderID;
        public string Name;
        public string ClanMasterName;
        public int CrestID;
        public byte[] CrestPicture;
        public byte[] CrestLargePicture;
        public int CastleID;
        public int HideoutID;
        public int ClanRank;
        public int ClanNameValue;
        public int AllianceID;
        public int InWar;
        public int JoinDominionWarID;

        public L2Alliance Alliance;

        public Hideout hideout;

        public List<ClanMember> members = new List<ClanMember>();
        public int LargeCrestID;
        public int ClanID;
        public int Status;
        public int Guilty;
        public int FortressID;

        public Dictionary<int, int> MainSkill = new Dictionary<int, int>();

        public e_ClanSub Knights_1;
        public e_ClanSub Knights_1_Order1;
        public e_ClanSub Knights_1_Order2;
        public e_ClanSub Knights_2;
        public e_ClanSub Knights_2_Order1;
        public e_ClanSub Knights_2_Order2;
        public e_ClanSub Academy;

        public static int CP_CL_JOIN_CLAN = 2;
        public static int CP_CL_GIVE_TITLE = 4;
        public static int CP_CL_VIEW_WAREHOUSE = 8;
        public static int CP_CL_MANAGE_RANKS = 16;
        public static int CP_CL_PLEDGE_WAR = 32;
        public static int CP_CL_DISMISS = 64;
        public static int CP_CL_REGISTER_CREST = 128;
        public static int CP_CL_APPRENTICE = 256;
        public static int CP_CL_TROOPS_FAME = 512;
        public static int CP_CL_SUMMON_AIRSHIP = 1024;
        public static int CP_CH_OPEN_DOOR = 2048;
        public static int CP_CH_OTHER_RIGHTS = 4096;
        public static int CP_CH_AUCTION = 8192;
        public static int CP_CH_DISMISS = 16384;
        public static int CP_CH_SET_FUNCTIONS = 32768;
        public static int CP_CS_OPEN_DOOR = 65536;
        public static int CP_CS_MANOR_ADMIN = 131072;
        public static int CP_CS_MANAGE_SIEGE = 262144;
        public static int CP_CS_USE_FUNCTIONS = 524288;
        public static int CP_CS_DISMISS = 1048576;
        public static int CP_CS_TAXES = 2097152;
        public static int CP_CS_MERCENARIES = 4194304;
        public static int CP_CS_SET_FUNCTIONS = 8388608;
        public static int CP_ALL = 16777214;

        public int AllianceCrestId
        {
            get { return Alliance == null ? 0 : Alliance.CrestID; }
        }

        public string AllianceName
        {
            get { return Alliance == null ? "" : Alliance.Name; }
        }

        public void addMember(L2Player player, short type)
        {
            ClanMember cm = new ClanMember();
            cm.classId = (byte)player.ActiveClass.ClassId.Id;
            cm.Level = player.Level;
            cm.Name = player.Name;
            cm.ObjID = player.ObjID;
            cm.sponsorId = 0;
            cm.NickName = player.Title;
            cm.ClanType = type;
            cm._pledgeTypeName = this.Name;
            cm.Target = player;

            members.Add(cm);

            player.Clan = this;
            player.ClanType = type;
        }

        public List<ClanMember> getClanMembers()
        {
            return members;
        }

        public void UpdatePledgeNameValue(int count)
        {
            ClanNameValue += count;
            //TODO packets
        }

        public void onEnter(L2Player player)
        {
            player.Clan = this;

            foreach (ClanMember m in members)
            {
                if (m.ObjID == player.ObjID)
                {
                    m.online = 1;
                    m.Level = player.Level;
                    m.Target = player;
                    break;
                }
            }

            if (LeaderID == player.ObjID)
                player.ClanPrivs = CP_ALL;

            player.sendPacket(new PledgeShowMemberListAll(this, e_ClanType.CLAN_MAIN));

            foreach (e_ClanSub sub in getAllSubs())
            {
                player.sendPacket(new PledgeReceiveSubPledgeCreated(sub));
                player.sendPacket(new PledgeShowMemberListAll(this, sub.Type));
            }
        }

        public void broadcastToMembers(GameServerNetworkPacket pk)
        {
            foreach (ClanMember cm in members)
                if (cm.online == 1)
                    cm.Target.sendPacket(pk);
        }

        public e_ClanType isSubLeader(int ObjID, e_ClanType[] types)
        {
            e_ClanType ret = e_ClanType.None;
            foreach (e_ClanType ct in types)
            {
                switch (ct)
                {
                    case e_ClanType.CLAN_KNIGHT1:
                        if (Knights_1 != null && Knights_1.LeaderID == ObjID)
                            ret = ct;
                        break;
                    case e_ClanType.CLAN_KNIGHT2:
                        if (Knights_2 != null && Knights_2.LeaderID == ObjID)
                            ret = ct;
                        break;
                    case e_ClanType.CLAN_KNIGHT3:
                        if (Knights_1_Order1 != null && Knights_1_Order1.LeaderID == ObjID)
                            ret = ct;
                        break;
                    case e_ClanType.CLAN_KNIGHT4:
                        if (Knights_1_Order2 != null && Knights_1_Order2.LeaderID == ObjID)
                            ret = ct;
                        break;
                    case e_ClanType.CLAN_KNIGHT5:
                        if (Knights_2_Order1 != null && Knights_2_Order1.LeaderID == ObjID)
                            ret = ct;
                        break;
                    case e_ClanType.CLAN_KNIGHT6:
                        if (Knights_2_Order2 != null && Knights_2_Order2.LeaderID == ObjID)
                            ret = ct;
                        break;
                }
            }

            return ret;
        }

        public bool IsDissolving()
        {
            return false;
        }

        public void updateCrest(int size, byte[] picture)
        {
            int msg = 1861; //The clan's crest has been deleted.
            CrestPicture = picture;

            if (size == 0)
            {
                if (CrestID > 0)
                    File.Delete(@"crests\c" + CrestID + ".bmp");

                CrestID = 0;
            }
            else
            {
                if (CrestID > 0)
                    File.Delete(@"crests\c" + CrestID + ".bmp");

                msg = 3140; //The crest was successfully registered.
                CrestID = IdFactory.Instance.nextId();
                try
                {
                    FileStream fs = new FileStream(@"crests\c" + CrestID + ".bmp", FileMode.Create, FileAccess.ReadWrite);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(picture);
                    bw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

            //SQL_Block sqb = new SQL_Block("clan_data");
            //sqb.param("crestId", CrestID);
            //sqb.where("id", ClanID);
            //sqb.sql_update(false);

            //foreach (ClanMember cm in members)
            //    if (cm.online == 1)
            //    {
            //        cm.Target.sendSystemMessage(msg);
            //        cm.Target.sendPacket(new PledgeCrest(CrestID, picture));
            //        cm.Target.broadcastUserInfo();
            //    }
        }

        public void updateCrestLarge(int size, byte[] picture)
        {
            int msg = 1861; //The clan's crest has been deleted.
            CrestLargePicture = picture;

            if (size == 0)
            {
                if (LargeCrestID > 0)
                    File.Delete(@"crests\b" + LargeCrestID + ".bmp");

                LargeCrestID = 0;
            }
            else
            {
                if (LargeCrestID > 0)
                    File.Delete(@"crests\b" + LargeCrestID + ".bmp");

                msg = 3140; //The crest was successfully registered.
                LargeCrestID = IdFactory.Instance.nextId();
                try
                {
                    FileStream fs = new FileStream(@"crests\b" + LargeCrestID + ".bmp", FileMode.Create, FileAccess.ReadWrite);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(picture);
                    bw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

            //SQL_Block sqb = new SQL_Block("clan_data");
            //sqb.param("crestBigId", LargeCrestID);
            //sqb.where("id", ClanID);
            //sqb.sql_update(false);

            //foreach (ClanMember cm in members)
            //    if (cm.online == 1)
            //    {
            //        cm.Target.sendSystemMessage(msg);
            //        cm.Target.sendPacket(new ExPledgeCrestLarge(LargeCrestID, picture));
            //        cm.Target.broadcastUserInfo();
            //    }
        }

        public byte MaxMembers(e_ClanType type)
        {
            byte val = 0;
            switch (type)
            {
                case e_ClanType.CLAN_ACADEMY:
                case e_ClanType.CLAN_KNIGHT1:
                case e_ClanType.CLAN_KNIGHT2:
                    val = 20;
                    break;
                case e_ClanType.CLAN_KNIGHT3:
                case e_ClanType.CLAN_KNIGHT4:
                case e_ClanType.CLAN_KNIGHT5:
                case e_ClanType.CLAN_KNIGHT6:
                    val = 10;
                    break;
                case e_ClanType.CLAN_MAIN:
                    switch (Level)
                    {
                        case 0:
                            val = 10;
                            break;
                        case 1:
                            val = 15;
                            break;
                        case 2:
                            val = 20;
                            break;
                        case 3:
                            val = 30;
                            break;

                        default:
                            val = 40;
                            break;
                    }
                    break;
            }

            return val;
        }

        public void Leave(L2Player player)
        {
            if (player.ObjID == LeaderID)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CLAN_LEADER_CANNOT_WITHDRAW);
                return;
            }

            e_ClanType type = isSubLeader(player.ObjID, new e_ClanType[] { e_ClanType.CLAN_KNIGHT1, e_ClanType.CLAN_KNIGHT2, e_ClanType.CLAN_KNIGHT3, e_ClanType.CLAN_KNIGHT4, e_ClanType.CLAN_KNIGHT5, e_ClanType.CLAN_KNIGHT6 });
            if (type != e_ClanType.None)
            {
                if (getClanMemberCount(type, player.ObjID) > 0)
                {
                    player.sendMessage("You are leader of clan sub unit, and while there some members - you cant leave them.");
                    return;
                }
            }

            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1_HAS_WITHDRAWN_FROM_THE_CLAN);
            sm.AddPlayerName(player.Name);
            broadcastToOnline(sm);

            foreach (ClanMember cm in members)
            {
                if (cm.ObjID == player.ObjID)
                {
                    lock (members)
                        members.Remove(cm);

                    break;
                }
            }

            player.Clan = null;
            player.ClanId = 0;
            player.ClanPrivs = 0;
            player.ClanType = 0;

            player.Title = "";
            player.sendSystemMessage(SystemMessage.SystemMessageId.YOU_HAVE_WITHDRAWN_FROM_CLAN);
            player.sendPacket(new PledgeShowMemberListDeleteAll());
            player.broadcastUserInfo();

            player.setPenalty_ClanJoin(DateTime.Now.AddHours(24), false);
            player.sendSystemMessage(SystemMessage.SystemMessageId.YOU_MUST_WAIT_BEFORE_JOINING_ANOTHER_CLAN);

            // player.updateDb();
        }

        private void broadcastToOnline(GameServerNetworkPacket p)
        {
            foreach (ClanMember cm in members)
                if (cm.online == 1)
                    cm.Target.sendPacket(p);
        }

        public byte getClanMemberCount(e_ClanType type, int myself)
        {
            byte cnt = 0;
            foreach (ClanMember cm in members)
                if (cm.ClanType == (short)type)
                {
                    if (myself != 0 && myself == cm.ObjID)
                        continue;

                    cnt++;
                }

            //  var x = from cm in members where cm.Type == type && cm.ObjID != myself let cnt2 = cnt+1 select cnt2;
            //  int cn = x.
            return cnt;
        }

        public List<ClanMember> getClanMembers(e_ClanType type, int myself)
        {
            List<ClanMember> mems = new List<ClanMember>();
            foreach (ClanMember cm in members)
                if (cm.ClanType == (short)type)
                {
                    if (myself != 0 && myself == cm.ObjID)
                        continue;

                    mems.Add(cm);
                }

            return mems;
        }

        public List<e_ClanSub> getAllSubs()
        {
            List<e_ClanSub> subs = new List<e_ClanSub>();
            if (Academy != null)
                subs.Add(Academy);
            if (Knights_1 != null)
                subs.Add(Knights_1);
            if (Knights_2 != null)
                subs.Add(Knights_2);
            if (Knights_1_Order1 != null)
                subs.Add(Knights_1_Order1);
            if (Knights_1_Order2 != null)
                subs.Add(Knights_1_Order2);
            if (Knights_2_Order1 != null)
                subs.Add(Knights_2_Order1);
            if (Knights_2_Order2 != null)
                subs.Add(Knights_2_Order2);

            return subs;
        }

        public void LevelUp(byte lvl)
        {
            Level = lvl;
        }

        public bool DominionLord()
        {
            return CastleID > 0; //TODO dom winner
        }

        public bool hasRights(L2Player player, int bits)
        {
            //TODO rights
            return false;
        }

        public bool HasSubPledge(int subId)
        {
            //TODO rights
            return false;
        }

        public string GetSubpledgeMasterName(int reply)
        {
            return null;
        }
    }

    public class ClanMember
    {
        public string Name;
        public byte Level;
        public byte classId;
        public int ObjID;
        public int sponsorId;
        public short ClanType;
        public string NickName,
                      _pledgeTypeName;
        public int ClanPrivs;
        public string _ownerName = "";
        public int Gender;
        public int Race;
        public int online;
        public L2Player Target;

        internal int haveMaster()
        {
            return 0;
        }

        public int OnlineID()
        {
            if (online == 0)
                return 0;

            return ObjID;
        }
    }

    public enum e_ClanRank
    {
        vagabond = 0,
        _1,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        _11,
    }

    public enum e_ClanType
    {
        CLAN_MAIN = 0,
        CLAN_KNIGHT1 = 100,
        CLAN_KNIGHT2 = 200,
        CLAN_KNIGHT3 = 1001,
        CLAN_KNIGHT4 = 1002,
        CLAN_KNIGHT5 = 2001,
        CLAN_KNIGHT6 = 2002,
        CLAN_ACADEMY = -1,
        None
    }

    public class e_ClanSub
    {
        public e_ClanType Type;
        public int LeaderID;
        public string LeaderName = "";
        public bool Enabled = false;
        public string Name;

        public e_ClanSub(e_ClanType e_ClanType)
        {
            this.Type = e_ClanType;
            Enabled = true;
        }
    }
}