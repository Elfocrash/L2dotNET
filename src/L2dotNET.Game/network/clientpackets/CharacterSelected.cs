using L2dotNET.GameService.Enums;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using Ninject;
using System.Linq;

namespace L2dotNET.GameService.network.l2recv
{
    class CharacterSelected : GameServerNetworkRequest
    {
        [Inject]
        public IPlayerService playerService { get { return GameServer.Kernel.Get<IPlayerService>(); } }

        public CharacterSelected(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _charSlot;

        private int _unk1;  // new in C4
        private int _unk2;  // new in C4
        private int _unk3;  // new in C4
        private int _unk4;	// new in C4

        public override void read()
        {
            _charSlot = readD();
            _unk1 = readH();
            _unk2 = readD();
            _unk3 = readD();
            _unk4 = readD();
        }

        public override void run()
        {
            GameClient client = getClient();

            PlayerModel playerModel = playerService.GetPlayerModelBySlotId(client.AccountName, _charSlot);
            L2Player player = getClient().AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            PlayerModelMapping(playerModel, player);

            player.Online = 1;
            player.Gameclient = client;
            client.CurrentPlayer = player;

            getClient().sendPacket(new l2send.CharacterSelected(player, client.SessionId));

        }

        private static void PlayerModelMapping(PlayerModel playerModel, L2Player player)
        {

            //AccountName = player.AccountName,
            //ObjectId = player.ObjID,
            //player.Name = playerModel.Name;
            player.Level = (byte)playerModel.Level;
            player.MaxHP = playerModel.MaxHp;
            player.CurHP = playerModel.CurHp;
            player.MaxCP = playerModel.MaxCp;
            player.CurCP = playerModel.CurCp;
            player.MaxMP = playerModel.MaxMp;
            player.CurMP = playerModel.CurMp;
            player.Face = playerModel.Face;
            player.HairStyle = playerModel.HairStyle;
            player.HairColor = playerModel.HairColor;
            player.Sex = (byte)playerModel.Sex;
            player.Heading = playerModel.Heading;
            player.X = playerModel.Heading;
            player.Y = playerModel.Y;
            player.Z = playerModel.Z;
            player.Exp = playerModel.Exp;
            player.ExpOnDeath = playerModel.ExpBeforeDeath;
            player.SP = playerModel.Sp;
            player.Karma = playerModel.Karma;
            player.PvpKills = playerModel.PvpKills;
            player.PkKills = playerModel.PkKills;
            player.ClanId = playerModel.ClanId;
            // player.BaseClass.ClassId.ClassRace = (ClassRace)playerModel.BaseClass;
            player.ActiveClass.ClassId.Id = (ClassIds)playerModel.ClassId;
            player.BaseClass.ClassId.Id = (ClassIds)playerModel.BaseClass;
            player.DeleteTime = playerModel.DeleteTime;
            player.CanCraft = playerModel.CanCraft;
            player.Title = playerModel.Title;
            player.RecHave = playerModel.RecHave;
            player.RecLeft = playerModel.RecLeft;
            player.AccessLevel = playerModel.AccessLevel;
            player.Online = playerModel.Online;
            player.OnlineTime = playerModel.OnlineTime;
            player.LastAccess = playerModel.LastAccess;
            player.ClanPrivs = playerModel.ClanPrivs;
            player.WantsPeace = playerModel.WantsPeace;
            player.IsIn7sDungeon = playerModel.IsIn7sDungeon;
            player.PunishLevel = playerModel.PunishLevel;
            player.PunishTimer = playerModel.PunishLevel;
            player.PowerGrade = playerModel.PowerGrade;
            player.Nobless = playerModel.Nobless;
            player.Hero = playerModel.Hero;
            player.Subpledge = playerModel.Subpledge;
            player.LastRecomDate = playerModel.LastRecomDate;
            player.LevelJoinedAcademy = playerModel.LevelJoinedAcademy;
            player.Apprentice = playerModel.Apprentice;
            player.Sponsor = playerModel.Sponsor;
            player.VarkaKetraAlly = playerModel.VarkaKetraAlly;
            player.ClanJoinExpiryTime = playerModel.ClanJoinExpiryTime;
            player.ClanCreateExpiryTime = playerModel.ClanCreateExpiryTime;
            player.DeathPenaltyLevel = playerModel.DeathPenaltyLevel;
        }
    }
}
