using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Structures;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.Network;

namespace L2dotNET.GameService.Model.Communities
{
    public class L2Clan
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(L2Clan));

        public byte Level;
        public int LeaderId;
        public string Name;
        public string ClanMasterName;
        public int CrestId;
        public byte[] CrestPicture;
        public byte[] CrestLargePicture;
        public int CastleId;
        public int HideoutId;
        public int ClanRank;
        public int ClanNameValue;
        public int AllianceId;
        public int InWar;
        public int JoinDominionWarId;

        public L2Alliance Alliance;

        public Hideout Hideout;

        public List<ClanMember> Members = new List<ClanMember>();
        public int LargeCrestId;
        public int ClanId;
        public int Status;
        public int Guilty;
        public int FortressId;

        public Dictionary<int, int> MainSkill = new Dictionary<int, int>();

        public EClanSub Knights1;
        public EClanSub Knights1Order1;
        public EClanSub Knights1Order2;
        public EClanSub Knights2;
        public EClanSub Knights2Order1;
        public EClanSub Knights2Order2;
        public EClanSub Academy;

        public static int CpClJoinClan = 2;
        public static int CpClGiveTitle = 4;
        public static int CpClViewWarehouse = 8;
        public static int CpClManageRanks = 16;
        public static int CpClPledgeWar = 32;
        public static int CpClDismiss = 64;
        public static int CpClRegisterCrest = 128;
        public static int CpClApprentice = 256;
        public static int CpClTroopsFame = 512;
        public static int CpClSummonAirship = 1024;
        public static int CpChOpenDoor = 2048;
        public static int CpChOtherRights = 4096;
        public static int CpChAuction = 8192;
        public static int CpChDismiss = 16384;
        public static int CpChSetFunctions = 32768;
        public static int CpCsOpenDoor = 65536;
        public static int CpCsManorAdmin = 131072;
        public static int CpCsManageSiege = 262144;
        public static int CpCsUseFunctions = 524288;
        public static int CpCsDismiss = 1048576;
        public static int CpCsTaxes = 2097152;
        public static int CpCsMercenaries = 4194304;
        public static int CpCsSetFunctions = 8388608;
        public static int CpAll = 16777214;

        public int AllianceCrestId => Alliance?.CrestId ?? 0;

        public string AllianceName => Alliance == null ? string.Empty : Alliance.Name;

        public void AddMember(L2Player player, short type)
        {
            ClanMember cm = new ClanMember
            {
                ClassId = (byte)player.ActiveClass.ClassId.Id,
                Level = player.Level,
                Name = player.Name,
                ObjId = player.ObjId,
                SponsorId = 0,
                NickName = player.Title,
                ClanType = type,
                PledgeTypeName = Name,
                Target = player
            };

            Members.Add(cm);

            player.Clan = this;
            player.ClanType = type;
        }

        public List<ClanMember> GetClanMembers()
        {
            return Members;
        }

        public void UpdatePledgeNameValue(int count)
        {
            ClanNameValue += count;
            //TODO packets
        }

        public void OnEnter(L2Player player)
        {
            player.Clan = this;

            ClanMember member = Members.FirstOrDefault(m => m.ObjId == player.ObjId);
            if (member != null)
            {
                member.Online = 1;
                member.Level = player.Level;
                member.Target = player;
            }

            if (LeaderId == player.ObjId)
                player.ClanPrivs = CpAll;

            player.SendPacket(new PledgeShowMemberListAll(this, EClanType.ClanMain));

            GetAllSubs().ForEach(sub =>
                                 {
                                     player.SendPacket(new PledgeReceiveSubPledgeCreated(sub));
                                     player.SendPacket(new PledgeShowMemberListAll(this, sub.Type));
                                 });
        }

        public void BroadcastToMembers(GameserverPacket pk)
        {
            foreach (ClanMember cm in Members.Where(cm => cm.Online == 1))
                cm.Target.SendPacket(pk);
        }

        //TODO: Simplify method body
        public EClanType IsSubLeader(int objId, IEnumerable<EClanType> types)
        {
            EClanType ret = EClanType.None;
            foreach (EClanType ct in types)
            {
                switch (ct)
                {
                    case EClanType.ClanKnight1:
                        if ((Knights1 != null) && (Knights1.LeaderId == objId))
                            ret = ct;
                        break;
                    case EClanType.ClanKnight2:
                        if ((Knights2 != null) && (Knights2.LeaderId == objId))
                            ret = ct;
                        break;
                    case EClanType.ClanKnight3:
                        if ((Knights1Order1 != null) && (Knights1Order1.LeaderId == objId))
                            ret = ct;
                        break;
                    case EClanType.ClanKnight4:
                        if ((Knights1Order2 != null) && (Knights1Order2.LeaderId == objId))
                            ret = ct;
                        break;
                    case EClanType.ClanKnight5:
                        if ((Knights2Order1 != null) && (Knights2Order1.LeaderId == objId))
                            ret = ct;
                        break;
                    case EClanType.ClanKnight6:
                        if ((Knights2Order2 != null) && (Knights2Order2.LeaderId == objId))
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

        public void UpdateCrest(int size, byte[] picture)
        {
            //SystemMessage.SystemMessageId msg;
            //msg = SystemMessage.SystemMessageId.CLAN_CREST_HAS_BEEN_DELETED;

            CrestPicture = picture;

            if (size == 0)
            {
                if (CrestId > 0)
                    File.Delete($@"crests\c{CrestId}.bmp");

                CrestId = 0;
            }
            else
            {
                if (CrestId > 0)
                    File.Delete($@"crests\c{CrestId}.bmp");

                //msg = SystemMessage.SystemMessageId.CLAN_CREST_WAS_SUCCESFULLY_REGISTERED;
                CrestId = IdFactory.Instance.NextId();
                try
                {
                    FileStream fs = new FileStream($@"crests\c{CrestId}.bmp", FileMode.Create, FileAccess.ReadWrite);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(picture);
                    bw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }

            //SQL_Block sqb = new SQL_Block("clan_data");
            //sqb.param("crestId", CrestID);
            //sqb.where("id", ClanID);
            //sqb.sql_update(false);

            //foreach (ClanMember cm in members.Where(m => m.online == 1))
            //{
            //   cm.Target.sendSystemMessage(msg);
            //   cm.Target.sendPacket(new PledgeCrest(CrestID, picture));
            //   cm.Target.broadcastUserInfo();
            //}
        }

        public void UpdateCrestLarge(int size, byte[] picture)
        {
            //SystemMessage.SystemMessageId msg;
            //msg = SystemMessage.SystemMessageId.CLAN_CREST_HAS_BEEN_DELETED;

            CrestLargePicture = picture;

            if (size == 0)
            {
                if (LargeCrestId > 0)
                    File.Delete($@"crests\b{LargeCrestId}.bmp");

                LargeCrestId = 0;
            }
            else
            {
                if (LargeCrestId > 0)
                    File.Delete($@"crests\b{LargeCrestId}.bmp");

                //msg = SystemMessage.SystemMessageId.CLAN_CREST_WAS_SUCCESFULLY_REGISTERED;
                LargeCrestId = IdFactory.Instance.NextId();
                try
                {
                    FileStream fs = new FileStream($@"crests\b{LargeCrestId}.bmp", FileMode.Create, FileAccess.ReadWrite);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(picture);
                    bw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }

            //SQL_Block sqb = new SQL_Block("clan_data");
            //sqb.param("crestBigId", LargeCrestID);
            //sqb.where("id", ClanID);
            //sqb.sql_update(false);

            //foreach (ClanMember cm in members.Where(m => m.online == 1))
            //{
            //   cm.Target.sendSystemMessage(msg);
            //   cm.Target.sendPacket(new ExPledgeCrestLarge(LargeCrestID, picture));
            //   cm.Target.broadcastUserInfo();
            //}
        }

        public byte MaxMembers(EClanType type)
        {
            byte val = 0;
            switch (type)
            {
                case EClanType.ClanAcademy:
                case EClanType.ClanKnight1:
                case EClanType.ClanKnight2:
                    val = 20;
                    break;
                case EClanType.ClanKnight3:
                case EClanType.ClanKnight4:
                case EClanType.ClanKnight5:
                case EClanType.ClanKnight6:
                    val = 10;
                    break;
                case EClanType.ClanMain:
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
            if (player.ObjId == LeaderId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ClanLeaderCannotWithdraw);
                return;
            }

            EClanType type = IsSubLeader(player.ObjId, new[] { EClanType.ClanKnight1, EClanType.ClanKnight2, EClanType.ClanKnight3, EClanType.ClanKnight4, EClanType.ClanKnight5, EClanType.ClanKnight6 });
            if (type != EClanType.None)
            {
                if (GetClanMemberCount(type, player.ObjId) > 0)
                {
                    player.SendMessage("You are leader of clan sub unit, and while there some members - you cant leave them.");
                    return;
                }
            }

            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1HasWithdrawnFromTheClan);
            sm.AddPlayerName(player.Name);
            BroadcastToOnline(sm);

            ClanMember cm = Members.FirstOrDefault(member => member.ObjId == player.ObjId);
            if (cm != null)
            {
                lock (Members)
                    Members.Remove(cm);
            }

            player.Clan = null;
            player.ClanId = 0;
            player.ClanPrivs = 0;
            player.ClanType = 0;

            player.Title = string.Empty;
            player.SendSystemMessage(SystemMessage.SystemMessageId.YouHaveWithdrawnFromClan);
            player.SendPacket(new PledgeShowMemberListDeleteAll());
            player.BroadcastUserInfo();

            player.setPenalty_ClanJoin(DateTime.Now.AddHours(24), false);
            player.SendSystemMessage(SystemMessage.SystemMessageId.YouMustWaitBeforeJoiningAnotherClan);

            // player.updateDb();
        }

        private void BroadcastToOnline(GameserverPacket p)
        {
            foreach (ClanMember cm in Members.Where(cm => cm.Online == 1))
                cm.Target.SendPacket(p);
        }

        public byte GetClanMemberCount(EClanType type, int myself)
        {
            return (byte)Members.Count(cm => (cm.ClanType == (short)type) && ((myself == 0) || (myself != cm.ObjId)));
        }

        public List<ClanMember> GetClanMembers(EClanType type, int myself)
        {
            return Members.Where(cm => (cm.ClanType == (short)type) && ((myself == 0) || (myself != cm.ObjId))).ToList();
        }

        public List<EClanSub> GetAllSubs()
        {
            List<EClanSub> subs = new List<EClanSub>();
            if (Academy != null)
                subs.Add(Academy);
            if (Knights1 != null)
                subs.Add(Knights1);
            if (Knights2 != null)
                subs.Add(Knights2);
            if (Knights1Order1 != null)
                subs.Add(Knights1Order1);
            if (Knights1Order2 != null)
                subs.Add(Knights1Order2);
            if (Knights2Order1 != null)
                subs.Add(Knights2Order1);
            if (Knights2Order2 != null)
                subs.Add(Knights2Order2);

            return subs;
        }

        public void LevelUp(byte lvl)
        {
            Level = lvl;
        }

        public bool DominionLord()
        {
            return CastleId > 0; //TODO dom winner
        }

        public bool HasRights(L2Player player, int bits)
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
}