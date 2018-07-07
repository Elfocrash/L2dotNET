using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Tables;

namespace L2dotNET.Utility
{
    public static class PlayerCharacterMap
    {
        public static CharacterContract ToContract(this L2Player player)
        {
            return new CharacterContract
                {
                    AccountId = player.Account.AccountId,
                    CharacterId = player.ObjectId,
                    Name = player.Name,
                    Level = player.Level,
                    MaxHp = player.MaxHp,
                    CurHp = (int) player.CharStatus.CurrentHp,
                    MaxCp = player.MaxCp,
                    CurCp = (int) player.CurrentCp,
                    MaxMp = player.MaxMp,
                    CurMp = (int) player.CharStatus.CurrentMp,
                    Face = (byte) player.Face,
                    HairStyle = (byte) player.HairStyleId,
                    HairColor = (byte) player.HairColor,
                    Sex = (byte) player.Sex,
                    Heading = player.Heading,
                    X = player.X,
                    Y = player.Y,
                    Z = player.Z,
                    Exp = player.Experience,
                    ExpBeforeDeath = player.ExpOnDeath,
                    Sp = player.Sp,
                    Karma = player.Karma,
                    PvpKills = player.PvpKills,
                    PkKills = player.PkKills,
                    Race = (int) player.BaseClass.ClassId.ClassRace,
                    ClassId = (int) player.ActiveClass.ClassId.Id,
                    BaseClass = (int) player.BaseClass.ClassId.Id,
                    DeleteTime = player.DeleteTime,
                    CanCraft = player.CanCraft,
                    Title = player.Title,
                    RecHave = player.RecomandationsHave,
                    RecLeft = player.RecomendationsLeft,
                    AccessLevel = player.AccessLevel,
                    Online = player.Online,
                    OnlineTime = player.OnlineTime,
                    CharSlot = player.CharacterSlot,
                    LastAccess = player.LastAccess,
                    PunishLevel = player.PunishLevel,
                    PunishTime = player.PunishTime,
                    PowerGrade = player.PowerGrade,
                    Nobless = player.Nobless,
                    Hero = player.Hero,
                    LastRecomDate = player.LastRecomendationDate
                };
        }

        public static L2Player ToPlayer(this CharacterContract characterContract)
        {
            L2Player player = new L2Player(CharTemplateTable.GetTemplate(characterContract.ClassId), characterContract.CharacterId)
                {
                    ObjectId = characterContract.CharacterId,
                    Name = characterContract.Name,
                    Title = characterContract.Title,
                    Level = (byte) characterContract.Level,
                    Face = (Face) characterContract.Face,
                    HairStyleId = (HairStyleId) characterContract.HairStyle,
                    HairColor = (HairColor) characterContract.HairColor,
                    Sex = (Gender) characterContract.Sex,
                    X = characterContract.X,
                    Y = characterContract.Y,
                    Z = characterContract.Z,
                    Heading = characterContract.Heading,
                    Experience = characterContract.Exp,
                    ExpOnDeath = characterContract.ExpBeforeDeath,
                    Sp = characterContract.Sp,
                    Karma = characterContract.Karma,
                    PvpKills = characterContract.PvpKills,
                    PkKills = characterContract.PkKills,
                    BaseClass = CharTemplateTable.GetTemplate(characterContract.BaseClass),
                    ActiveClass = CharTemplateTable.GetTemplate(characterContract.ClassId),
                    RecomendationsLeft = characterContract.RecLeft,
                    RecomandationsHave = characterContract.RecHave,
                    CharacterSlot = characterContract.CharSlot,
                    DeleteTime = characterContract.DeleteTime,
                    LastAccess = characterContract.LastAccess,
                    CanCraft = characterContract.CanCraft,
                    AccessLevel = characterContract.AccessLevel,
                    OnlineTime = characterContract.OnlineTime,
                    PunishLevel = characterContract.PunishLevel,
                    PunishTime = characterContract.PunishTime,
                    PowerGrade = characterContract.PowerGrade,
                    Nobless = characterContract.Nobless,
                    Hero = characterContract.Hero,
                    LastRecomendationDate = characterContract.LastRecomDate
                };
            player.CharStatus.SetCurrentCp(characterContract.CurCp,
                false); //player.CharStatus.CurrentCp = playerContract.CurCp; //???after repairing the broadcast, return it back???
            player.CharStatus.SetCurrentHp(characterContract.CurHp, false); //player.CharStatus.CurrentHp = playerContract.CurHp;
            player.CharStatus.SetCurrentMp(characterContract.CurMp, false); //player.CharStatus.CurrentMp = playerContract.CurMp;

            return player;
        }
    }
}
