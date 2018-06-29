using System.Data;
using System.Linq;
using Dapper;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Repositories.Contracts;
using MySql.Data.MySqlClient;

namespace L2dotNET.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        internal IDbConnection Db;

        public PlayerRepository()
        {
            Db = new MySqlConnection("Server=127.0.0.1;Database=l2dotnet;Uid=l2dotnet;Pwd=l2dotnet;SslMode=none;");
        }

        public CharacterContract GetPlayerByLogin(int objId)
        {
            try
            {
                return Db.Query<CharacterContract>(@"select account_name as AccountName, obj_Id as ObjectId, char_name as Name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
                Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,base_class as BaseClass,classid as ClassId, DeleteTime,CanCraft,Title,
                rec_have as RecHave,rec_left as RecLeft,AccessLevel,punish_level as PunishLevel,punish_timer as PunishTimer,
                power_grade as PowerGrade,Nobless,Hero,last_recom_date as LastRecomDate,char_slot as CharSlot
                , lastAccess from characters where obj_Id=@id", new
                {
                    id = objId
                }).FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(GetPlayerByLogin)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
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

        public void CreatePlayer(CharacterContract character)
        {
            try
            {
                Db.Execute(@"insert into characters (account_name, obj_Id, char_name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
                             Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,Race,classid,base_class, DeleteTime,CanCraft,Title,
                             rec_have,rec_left,AccessLevel,char_slot,punish_level,punish_timer, power_grade,Nobless,Hero,
                             last_recom_date) 
                             Values (@account_name, @obj_Id, @char_name, @Level, @MaxHp, @CurHp, @MaxCp, @CurCp,@MaxMp,@CurMp,
                             @Face,@HairStyle,@HairColor,@Sex,@Heading,@X,@Y,@Z,@Exp,@ExpBeforeDeath,@Sp,@Karma,@PvpKills,@PkKills,@Race,@classid,@base_class, @DeleteTime,@CanCraft,@Title,
                             @rec_have,@rec_left,@AccessLevel,@char_slot,@punish_level,@punish_timer, @power_grade,@Nobless,@Hero,
                             @last_recom_date)", new
                {
                    account_name = character.AccountName,
                    obj_Id = character.ObjectId,
                    char_name = character.Name,
                    Level = character.Level,
                    MaxHp = character.MaxHp,
                    CurHp = character.CurHp,
                    MaxCp = character.MaxCp,
                    CurCp = character.CurCp,
                    MaxMp = character.MaxMp,
                    CurMp = character.CurMp,
                    Face = character.Face,
                    HairStyle = character.HairStyle,
                    HairColor = character.HairColor,
                    Sex = character.Sex,
                    Heading = character.Heading,
                    X = character.X,
                    Y = character.Y,
                    Z = character.Z,
                    Exp = character.Exp,
                    ExpBeforeDeath = character.ExpBeforeDeath,
                    Sp = character.Sp,
                    Karma = character.Karma,
                    PvpKills = character.PvpKills,
                    PkKills = character.PkKills,
                    Race = character.Race,
                    classid = character.ClassId,
                    base_class = character.BaseClass,
                    DeleteTime = character.DeleteTime,
                    CanCraft = character.CanCraft,
                    Title = character.Title,
                    rec_have = character.RecHave,
                    rec_left = character.RecLeft,
                    AccessLevel = character.AccessLevel,
                    char_slot = character.CharSlot,
                    punish_level = character.PunishLevel,
                    punish_timer = character.PunishTimer,
                    power_grade = character.PowerGrade,
                    Nobless = character.Nobless,
                    Hero = character.Hero,
                    last_recom_date = character.LastRecomDate
                });
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(CreatePlayer)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
            }
        }

        public void UpdatePlayer(CharacterContract character)
        {
            try
            {
                Db.ExecuteAsync(@"UPDATE characters SET level=@level,maxHp=@maxHp,curHp=@curHp,maxCp=@maxCp,curCp=@curCp,maxMp=@maxMp,curMp=@curMp,face=@face,hairStyle=@hairStyle,
                             hairColor=@hairColor,sex=@sex,heading=@heading
                             ,x=@x,y=@y,z=@z,exp=@exp,expBeforeDeath=@expBeforeDeath,sp=@sp,karma=@karma,pvpkills=@pvpkills,pkkills=@pkkills,rec_have=@rec_have,rec_left=@rec_left,
                             race=@race,classid=@classid,deletetime=@deletetime,title=@title,accesslevel=@accesslevel
                             ,online=@online,base_class=@base_class,onlinetime=@onlinetime,punish_level=@punish_level,
                             punish_timer=@punish_timer,nobless=@nobless,power_grade=@power_grade,
                             last_recom_date=@last_recom_date, lastAccess=@lastAccess WHERE obj_id=@obj_id", new
                {
                    level = character.Level,
                    maxHp = character.MaxHp,
                    curHp = character.CurHp,
                    maxCp = character.MaxCp,
                    curCp = character.CurCp,
                    maxMp = character.MaxMp,
                    curMp = character.CurMp,
                    face = character.Face,
                    hairStyle = character.HairStyle,
                    hairColor = character.HairColor,
                    sex = character.Sex,
                    heading = character.Heading,
                    x = character.X,
                    y = character.Y,
                    z = character.Z,
                    exp = character.Exp,
                    expBeforeDeath = character.ExpBeforeDeath,
                    sp = character.Sp,
                    karma = character.Karma,
                    pvpkills = character.PvpKills,
                    pkkills = character.PkKills,
                    online = character.Online,
                    onlinetime = character.OnlineTime,
                    race = character.Race,
                    classid = character.ClassId,
                    base_class = character.BaseClass,
                    deletetime = character.DeleteTime,
                    canCraft = character.CanCraft,
                    title = character.Title,
                    rec_have = character.RecHave,
                    rec_left = character.RecLeft,
                    accesslevel = character.AccessLevel,
                    punish_level = character.PunishLevel,
                    punish_timer = character.PunishTimer,
                    power_grade = character.PowerGrade,
                    nobless = character.Nobless,
                    last_recom_date = character.LastRecomDate,
                    lastAccess = character.LastAccess,
                    obj_Id = character.ObjectId
                });
            }
            catch (MySqlException ex)
            {
                Log.Error($"Method: {nameof(UpdatePlayer)}. Message: \'{ex.Message}\' (Error Number: \'{ex.Number}\')");
            }
        }

        public CharacterContract GetPlayerModelBySlotId(string accountName, int slotId)
        {
            try
            {
                return Db.Query<CharacterContract>(@"select obj_Id as ObjectId, char_name as Name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
                                               Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,base_class as BaseClass,classid as ClassId, DeleteTime,CanCraft,Title,
                                               rec_have as RecHave,rec_left as RecLeft,AccessLevel,punish_level as PunishLevel,punish_timer as PunishTimer,
                                               power_grade as PowerGrade,Nobless,Hero,last_recom_date as LastRecomDate
                                                , lastAccess from characters where account_name=@account_name AND char_slot=@char_slot", new
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