using System.Linq;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CharacterSelected : GameServerNetworkRequest
    {
        [Inject]
        public IPlayerService PlayerService
        {
            get { return GameServer.Kernel.Get<IPlayerService>(); }
        }

        public CharacterSelected(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _charSlot;

        private int _unk1; // new in C4
        private int _unk2; // new in C4
        private int _unk3; // new in C4
        private int _unk4; // new in C4

        public override void Read()
        {
            _charSlot = ReadD();
            _unk1 = ReadH();
            _unk2 = ReadD();
            _unk3 = ReadD();
            _unk4 = ReadD();
        }

        public override void Run()
        {
            GameClient client = GetClient();

            PlayerModel playerModel = PlayerService.GetPlayerModelBySlotId(client.AccountName, _charSlot);
            L2Player player = GetClient().AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            PlayerModelMapping(playerModel, player);

            if (player != null)
            {
                player.Online = 1;
                player.Gameclient = client;
                client.CurrentPlayer = player;

                GetClient().SendPacket(new Serverpackets.CharacterSelected(player, client.SessionId));
            }
        }

        private static void PlayerModelMapping(PlayerModel playerModel, L2Player player)
        {
            //AccountName = player.AccountName,
            //ObjectId = player.ObjID,
            //player.Name = playerModel.Name;
            player.Level = (byte)playerModel.Level;
            player.MaxHp = playerModel.MaxHp;
            player.CurHp = playerModel.CurHp;
            player.MaxCp = playerModel.MaxCp;
            player.CurCp = playerModel.CurCp;
            player.MaxMp = playerModel.MaxMp;
            player.CurMp = playerModel.CurMp;
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
            player.Sp = playerModel.Sp;
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
            player.IsIn7SDungeon = playerModel.IsIn7SDungeon;
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