using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using System.Collections.Generic;
using L2dotNET.DataContracts;
using L2dotNET.Enums;
using L2dotNET.model.inventory;
using L2dotNET.model.player;
using L2dotNET.model.player.General;
using L2dotNET.tables;

namespace L2dotNET.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public L2Player GetPlayerByLogin(int objId)
        {
            var playerContract = _unitOfWork.PlayerRepository.GetPlayerByLogin(objId);
            //TODO Use automapper to map this
            var player = new L2Player(objId, CharTemplateTable.Instance.GetTemplate(playerContract.ClassId))
            {
                ObjId = objId,
                Name = playerContract.Name,
                Title = playerContract.Title,
                Level = (byte)playerContract.Level,
                //MaxHp = playerContract.MaxHp,
                //MaxCp = playerContract.MaxCp,
                //MaxMp = playerContract.MaxMp,
                Face = (Face)playerContract.Face,
                HairStyleId = (HairStyleId)playerContract.HairStyle,
                HairColor = (HairColor)playerContract.HairColor,
                Sex = (Gender)playerContract.Sex,
                X = playerContract.X,
                Y = playerContract.Y,
                Z = playerContract.Z,
                Heading = playerContract.Heading,
                Exp = playerContract.Exp,
                ExpOnDeath = playerContract.ExpBeforeDeath,
                Sp = playerContract.Sp,
                Karma = playerContract.Karma,
                PvpKills = playerContract.PvpKills,
                PkKills = playerContract.PkKills,
                BaseClass = CharTemplateTable.Instance.GetTemplate(playerContract.BaseClass),
                ActiveClass = CharTemplateTable.Instance.GetTemplate(playerContract.ClassId),
                //ClassId = playerContract.ClassId,
                RecLeft = playerContract.RecLeft,
                RecHave = playerContract.RecHave,
                CharSlot = playerContract.CharSlot,
                Inventory = new PcInventory(null),
                DeleteTime = playerContract.DeleteTime,
                LastAccess = playerContract.LastAccess,
                CanCraft = playerContract.CanCraft,
                AccessLevel = playerContract.AccessLevel,
                Online = playerContract.Online,
                OnlineTime = playerContract.OnlineTime,
                PunishLevel = playerContract.PunishLevel,
                PunishTimer = playerContract.PunishTimer,
                PowerGrade = playerContract.PowerGrade,
                Nobless = playerContract.Nobless,
                Hero = playerContract.Hero,
                LastRecomDate = playerContract.LastRecomDate
            };
            player.Status.CurrentCp = playerContract.CurCp;
            player.Status.CurrentHp = playerContract.CurHp;
            player.Status.CurrentMp = playerContract.CurMp;

            return player;
        }

        public bool CheckIfPlayerNameExists(string name)
        {
            return _unitOfWork.PlayerRepository.CheckIfPlayerNameExists(name);
        }

        public void CreatePlayer(L2Player player)
        {
            // TODO Use automapper to map this
            var playerContract = new PlayerContract
            {
                AccountName = player.AccountName,
                ObjectId = player.ObjId,
                Name = player.Name,
                Level = player.Level,
                MaxHp = player.MaxHp,
                CurHp = (int)player.CurHp,
                MaxCp = player.MaxCp,
                CurCp = (int)player.CurCp,
                MaxMp = player.MaxMp,
                CurMp = (int)player.CurMp,
                Face = (int)player.Face,
                HairStyle = (int)player.HairStyleId,
                HairColor = (int)player.HairColor,
                Sex = (int)player.Sex,
                Heading = player.Heading,
                X = player.X,
                Y = player.Y,
                Z = player.Z,
                Exp = player.Exp,
                ExpBeforeDeath = player.ExpOnDeath,
                Sp = player.Sp,
                Karma = player.Karma,
                PvpKills = player.PvpKills,
                PkKills = player.PkKills,
                Race = (int)player.BaseClass.ClassId.ClassRace,
                ClassId = (int)player.ActiveClass.ClassId.Id,
                BaseClass = (int)player.BaseClass.ClassId.Id,
                DeleteTime = player.DeleteTime,
                CanCraft = player.CanCraft,
                Title = player.Title,
                RecHave = player.RecHave,
                RecLeft = player.RecLeft,
                AccessLevel = player.AccessLevel,
                Online = player.Online,
                OnlineTime = player.OnlineTime,
                CharSlot = player.CharSlot,
                LastAccess = player.LastAccess,
                PunishLevel = player.PunishLevel,
                PunishTimer = player.PunishTimer,
                PowerGrade = player.PowerGrade,
                Nobless = player.Nobless,
                Hero = player.Hero,
                LastRecomDate = player.LastRecomDate
            };
            _unitOfWork.PlayerRepository.CreatePlayer(playerContract);
        }

        public void UpdatePlayer(L2Player player)
        {
            //TODO Use automapper to map this
            PlayerContract playerContract = new PlayerContract
            {
                ObjectId = player.ObjId,
                Level = player.Level,
                MaxHp = player.MaxHp,
                CurHp = (int)player.CurHp,
                MaxCp = player.MaxCp,
                CurCp = (int)player.CurCp,
                MaxMp = player.MaxMp,
                CurMp = (int)player.CurMp,
                Face = (int)player.Face,
                HairStyle = (int)player.HairStyleId,
                HairColor = (int)player.HairColor,
                Sex = (int)player.Sex,
                Heading = player.Heading,
                X = player.X,
                Y = player.Y,
                Z = player.Z,
                Exp = player.Exp,
                ExpBeforeDeath = player.ExpOnDeath,
                Sp = player.Sp,
                Karma = player.Karma,
                PvpKills = player.PvpKills,
                PkKills = player.PkKills,
                Race = (int)player.BaseClass.ClassId.ClassRace,
                ClassId = (int)player.ActiveClass.ClassId.Id,
                BaseClass = (int)player.BaseClass.ClassId.Id,
                DeleteTime = player.DeleteTime,
                CanCraft = player.CanCraft,
                Title = player.Title,
                RecHave = player.RecHave,
                RecLeft = player.RecLeft,
                AccessLevel = player.AccessLevel,
                PunishLevel = player.PunishLevel,
                PunishTimer = player.PunishTimer,
                PowerGrade = player.PowerGrade,
                Nobless = player.Nobless,
                LastAccess = player.LastAccess
            };
            _unitOfWork.PlayerRepository.UpdatePlayer(playerContract);
        }

        public L2Player GetPlayerBySlotId(string accountName, int slotId)
        {
            var playerContract = _unitOfWork.PlayerRepository.GetPlayerModelBySlotId(accountName, slotId);
            var player = RestorePlayer(playerContract.ObjectId, null);
            return player;
        }

        public bool MarkToDeleteChar(int objId, long deletetime)
        {
            return _unitOfWork.PlayerRepository.MarkToDeleteChar(objId, deletetime);
        }

        public bool MarkToRestoreChar(int objId)
        {
            return _unitOfWork.PlayerRepository.MarkToRestoreChar(objId);
        }

        public bool DeleteCharByObjId(int objId)
        {
            return _unitOfWork.PlayerRepository.DeleteCharByObjId(objId);
        }

        public List<SkillResponseContract> GetPlayerSkills(int objId)
        {
            return _unitOfWork.SkillRepository.GetPlayerSkills(objId);
        }

        public L2Player RestorePlayer(int id, GameClient client)
        {
            var player = GetPlayerByLogin(id);

            player.Gameclient = client;
            //player.CStatsInit();
            player.Inventory.Restore(player);
            player.SessionData = new PlayerBag();

            return player;
        }
    }
}