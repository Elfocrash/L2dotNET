using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using log4net;
using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using MySql.Data.MySqlClient;

namespace L2dotNET.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerRepository));

        internal IDbConnection Db;

        public PlayerRepository()
        {
            Db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public PlayerModel GetAccountByLogin(int objId)
        {
            try
            {
                return Db.Query<PlayerModel>(@"select account_name as AccountName, obj_Id as ObjectId, char_name as Name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
                Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,base_class as BaseClass, DeleteTime,CanCraft,Title,
                rec_have as RecHave,rec_left as RecLeft,AccessLevel,clan_privs as ClanPrivs, WantsPeace,punish_level as PunishLevel,punish_timer as PunishTimer,
                power_grade as PowerGrade,Nobless,Hero,Subpledge,last_recom_date as LastRecomDate,char_slot as CharSlot,lvl_joined_academy as LevelJoinedAcademy, Apprentice, Sponsor,
                varka_ketra_ally as VarkaKetraAlly,clan_join_expiry_time as ClanJoinExpiryTime,clan_create_expiry_time as ClanCreateExpiryTime, death_penalty_level as
                DeathPenaltyLevel, lastAccess from characters where obj_Id=@id", new
                {
                    id = objId
                }).FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(GetAccountByLogin)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
                return null;
            }
        }

        public bool CheckIfPlayerNameExists(string name)
        {
            try
            {
                return Db.Query("select 1 from characters where char_name=@name limit 1", new
                {
                    name = name
                }).Any();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(CheckIfPlayerNameExists)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
                return true;
            }
        }

        public void CreatePlayer(PlayerModel player)
        {
            try
            {
                Db.Execute(@"insert into characters (account_name, obj_Id, char_name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
                             Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,Race,classid,base_class, DeleteTime,CanCraft,Title,
                             rec_have,rec_left,AccessLevel,char_slot,clan_privs, WantsPeace,punish_level,punish_timer, power_grade,Nobless,Hero,Subpledge,
                             last_recom_date,lvl_joined_academy, Apprentice, Sponsor,varka_ketra_ally,clan_join_expiry_time,clan_create_expiry_time, death_penalty_level) 
                             Values (@account_name, @obj_Id, @char_name, @Level, @MaxHp, @CurHp, @MaxCp, @CurCp,@MaxMp,@CurMp,
                             @Face,@HairStyle,@HairColor,@Sex,@Heading,@X,@Y,@Z,@Exp,@ExpBeforeDeath,@Sp,@Karma,@PvpKills,@PkKills,@Race,@classid,@base_class, @DeleteTime,@CanCraft,@Title,
                             @rec_have,@rec_left,@AccessLevel,@char_slot,@clan_privs, @WantsPeace,@punish_level,@punish_timer, @power_grade,@Nobless,@Hero,@Subpledge,
                             @last_recom_date,@lvl_joined_academy, @Apprentice, @Sponsor,@varka_ketra_ally,@clan_join_expiry_time,@clan_create_expiry_time, @death_penalty_level)", new
                {
                    account_name = player.AccountName,
                    obj_Id = player.ObjectId,
                    char_name = player.Name,
                    Level = player.Level,
                    MaxHp = player.MaxHp,
                    CurHp = player.CurHp,
                    MaxCp = player.MaxCp,
                    CurCp = player.CurCp,
                    MaxMp = player.MaxMp,
                    CurMp = player.CurMp,
                    Face = player.Face,
                    HairStyle = player.HairStyle,
                    HairColor = player.HairColor,
                    Sex = player.Sex,
                    Heading = player.Heading,
                    X = player.X,
                    Y = player.Y,
                    Z = player.Z,
                    Exp = player.Exp,
                    ExpBeforeDeath = player.ExpBeforeDeath,
                    Sp = player.Sp,
                    Karma = player.Karma,
                    PvpKills = player.PvpKills,
                    PkKills = player.PkKills,
                    Race = player.Race,
                    classid = player.ClassId,
                    base_class = player.BaseClass,
                    DeleteTime = player.DeleteTime,
                    CanCraft = player.CanCraft,
                    Title = player.Title,
                    rec_have = player.RecHave,
                    rec_left = player.RecLeft,
                    AccessLevel = player.AccessLevel,
                    char_slot = player.CharSlot,
                    clan_privs = player.ClanPrivs,
                    WantsPeace = player.WantsPeace,
                    punish_level = player.PunishLevel,
                    punish_timer = player.PunishTimer,
                    power_grade = player.PowerGrade,
                    Nobless = player.Nobless,
                    Hero = player.Hero,
                    Subpledge = player.Subpledge,
                    last_recom_date = player.LastRecomDate,
                    lvl_joined_academy = player.LevelJoinedAcademy,
                    Apprentice = player.Apprentice,
                    Sponsor = player.Sponsor,
                    varka_ketra_ally = player.VarkaKetraAlly,
                    clan_join_expiry_time = player.ClanJoinExpiryTime,
                    clan_create_expiry_time = player.ClanCreateExpiryTime,
                    death_penalty_level = player.DeathPenaltyLevel
                });
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(CreatePlayer)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
            }
        }

        public void UpdatePlayer(PlayerModel player)
        {
            try
            {
                Db.ExecuteAsync(@"UPDATE characters SET level=@level,maxHp=@maxHp,curHp=@curHp,maxCp=@maxCp,curCp=@curCp,maxMp=@maxMp,curMp=@curMp,face=@face,hairStyle=@hairStyle,
                             hairColor=@hairColor,sex=@sex,heading=@heading
                             ,x=@x,y=@y,z=@z,exp=@exp,expBeforeDeath=@expBeforeDeath,sp=@sp,karma=@karma,pvpkills=@pvpkills,pkkills=@pkkills,rec_have=@rec_have,rec_left=@rec_left,
                             race=@race,classid=@classid,deletetime=@deletetime,title=@title,accesslevel=@accesslevel
                             ,online=@online,clan_privs=@clan_privs,wantspeace=@wantspeace,base_class=@base_class,onlinetime=@onlinetime,punish_level=@punish_level,
                             punish_timer=@punish_timer,nobless=@nobless,power_grade=@power_grade,subpledge=@subpledge,
                             last_recom_date=@last_recom_date,lvl_joined_academy=@lvl_joined_academy,apprentice=@apprentice,sponsor=@sponsor,varka_ketra_ally=@varka_ketra_ally,
                             clan_join_expiry_time=@clan_join_expiry_time,clan_create_expiry_time=@clan_create_expiry_time,
                             death_penalty_level=@death_penalty_level, lastAccess=@lastAccess WHERE obj_id=@obj_id", new
                {
                    level = player.Level,
                    maxHp = player.MaxHp,
                    curHp = player.CurHp,
                    maxCp = player.MaxCp,
                    curCp = player.CurCp,
                    maxMp = player.MaxMp,
                    curMp = player.CurMp,
                    face = player.Face,
                    hairStyle = player.HairStyle,
                    hairColor = player.HairColor,
                    sex = player.Sex,
                    heading = player.Heading,
                    x = player.X,
                    y = player.Y,
                    z = player.Z,
                    exp = player.Exp,
                    expBeforeDeath = player.ExpBeforeDeath,
                    sp = player.Sp,
                    karma = player.Karma,
                    pvpkills = player.PvpKills,
                    pkkills = player.PkKills,
                    online = player.Online,
                    onlinetime = player.OnlineTime,
                    race = player.Race,
                    classid = player.ClassId,
                    base_class = player.BaseClass,
                    deletetime = player.DeleteTime,
                    canCraft = player.CanCraft,
                    title = player.Title,
                    rec_have = player.RecHave,
                    rec_left = player.RecLeft,
                    accesslevel = player.AccessLevel,
                    clan_privs = player.ClanPrivs,
                    wantspeace = player.WantsPeace,
                    punish_level = player.PunishLevel,
                    punish_timer = player.PunishTimer,
                    power_grade = player.PowerGrade,
                    nobless = player.Nobless,
                    subpledge = player.Subpledge,
                    last_recom_date = player.LastRecomDate,
                    lvl_joined_academy = player.LevelJoinedAcademy,
                    apprentice = player.Apprentice,
                    sponsor = player.Sponsor,
                    varka_ketra_ally = player.VarkaKetraAlly,
                    clan_join_expiry_time = player.ClanJoinExpiryTime,
                    clan_create_expiry_time = player.ClanCreateExpiryTime,
                    death_penalty_level = player.DeathPenaltyLevel,
                    lastAccess = player.LastAccess,
                    obj_Id = player.ObjectId
                });
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(UpdatePlayer)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
            }
        }

        public PlayerModel GetPlayerModelBySlotId(string accountName, int slotId)
        {
            try
            {
                return Db.Query<PlayerModel>(@"select obj_Id as ObjectId, char_name as Name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
                                               Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,base_class as BaseClass,classid as ClassId, DeleteTime,CanCraft,Title,
                                               rec_have as RecHave,rec_left as RecLeft,AccessLevel,clan_privs as ClanPrivs, WantsPeace,punish_level as PunishLevel,punish_timer as PunishTimer,
                                               power_grade as PowerGrade,Nobless,Hero,Subpledge,last_recom_date as LastRecomDate,lvl_joined_academy as LevelJoinedAcademy, Apprentice, Sponsor,
                                               varka_ketra_ally as VarkaKetraAlly,clan_join_expiry_time as ClanJoinExpiryTime,clan_create_expiry_time as ClanCreateExpiryTime, death_penalty_level as
                                               DeathPenaltyLevel, lastAccess from characters where account_name=@account_name AND char_slot=@char_slot", new
                {
                    account_name = accountName,
                    char_slot = slotId
                }).FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(GetPlayerModelBySlotId)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
                return null;
            }
        }

        public bool MarkToDeleteChar(int objId, long deletetime)
        {
            bool success;
            try
            {
                Db.Execute(@"UPDATE characters SET deletetime=@deletetime WHERE obj_id=@obj_id", new
                {
                    deletetime = deletetime,
                    obj_id = objId
                });

                success = true;
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(MarkToDeleteChar)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");

                success = false;
            }
            return success;
        }

        public bool MarkToRestoreChar(int objId)
        {
            bool success;
            try
            {
                Db.Execute(@"UPDATE characters SET deletetime=0 WHERE obj_id=@obj_id", new
                {
                    obj_id = objId
                });

                success = true;
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(MarkToRestoreChar)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");

                success = false;
            }
            return success;
        }

        public bool DeleteCharByObjId(int objId)
        {
            bool success;
            try
            {
                //this.db.Execute(@"DELETE FROM character_friends WHERE char_id=@obj_id OR friend_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_hennas WHERE char_obj_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_macroses WHERE char_obj_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_quests WHERE charId=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_recipebook WHERE char_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_shortcuts WHERE char_obj_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_skills WHERE char_obj_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_skills_save WHERE char_obj_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_subclasses WHERE char_obj_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM heroes WHERE char_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM olympiad_nobles WHERE char_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM seven_signs WHERE char_obj_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM pets WHERE item_obj_id IN (SELECT object_id FROM items WHERE items.owner_id=@obj_id)", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM augmentations WHERE item_id IN (SELECT object_id FROM items WHERE items.owner_id=@obj_id)", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM items WHERE owner_id=@obj_id", new { obj_id = objId });
                //this.db.Execute(@"DELETE FROM character_raid_points WHERE char_id=@obj_id", new { obj_id = objId });
                Db.Execute(@"DELETE FROM characters WHERE obj_Id=@obj_id", new
                {
                    obj_id = objId
                });

                success = true;
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(DeleteCharByObjId)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");

                success = false;
            }
            return success;
        }
    }
}