using L2dotNET.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using System.Configuration;
using L2dotNET.Models;

namespace L2dotNET.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        internal IDbConnection db;

        public PlayerRepository()
        {
            this.db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public PlayerModel GetAccountByLogin(int objId)
        {
            return this.db.Query<PlayerModel>(@"select account_name as AccountName, obj_Id as ObjectId, char_name as Name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
               Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,ClanId,base_class as BaseClass, DeleteTime,CanCraft,Title,
                rec_have as RecHave,rec_left as RecLeft,AccessLevel,clan_privs as ClanPrivs, WantsPeace,IsIn7sDungeon,punish_level as PunishLevel,punish_timer as PunishTimer,
                power_grade as PowerGrade,Nobless,Hero,Subpledge,last_recom_date as LastRecomDate,lvl_joined_academy as LevelJoinedAcademy, Apprentice, Sponsor,
                varka_ketra_ally as VarkaKetraAlly,clan_join_expiry_time as ClanJoinExpiryTime,clan_create_expiry_time as ClanCreateExpiryTime, death_penalty_level as
                DeathPenaltyLevel from characters where obj_Id=@id",
                new { id = objId }).FirstOrDefault();
        }

        public bool CheckIfPlayerNameExists(string name)
        {
            return this.db.Query<bool>("select count(*) from characters where char_name=@name", new { name = name }).FirstOrDefault();
        }

        public void CreatePlayer(PlayerModel player)
        {
            this.db.Execute(@"insert into characters (account_name, obj_Id, char_name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
               Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,ClanId,Race,classid,base_class, DeleteTime,CanCraft,Title,
                rec_have,rec_left,AccessLevel,clan_privs, WantsPeace,IsIn7sDungeon,punish_level,punish_timer, power_grade,Nobless,Hero,Subpledge,
                last_recom_date,lvl_joined_academy, Apprentice, Sponsor,varka_ketra_ally,clan_join_expiry_time,clan_create_expiry_time, death_penalty_level) 
                Values (@account_name, @obj_Id, @char_name, @Level, @MaxHp, @CurHp, @MaxCp, @CurCp,@MaxMp,@CurMp,
               @Face,@HairStyle,@HairColor,@Sex,@Heading,@X,@Y,@Z,@Exp,@ExpBeforeDeath,@Sp,@Karma,@PvpKills,@PkKills,@ClanId,@Race,@classid,@base_class, @DeleteTime,@CanCraft,@Title,
                @rec_have,@rec_left,@AccessLevel,@clan_privs, @WantsPeace,@IsIn7sDungeon,@punish_level,@punish_timer, @power_grade,@Nobless,@Hero,@Subpledge,
                @last_recom_date,@lvl_joined_academy, @Apprentice, @Sponsor,@varka_ketra_ally,@clan_join_expiry_time,@clan_create_expiry_time, @death_penalty_level)", 
                new { account_name = player.AccountName, obj_Id = player.ObjectId, char_name = player.Name, Level = player.Level, MaxHp = player.MaxHp, CurHp = player.CurHp,
                MaxCp = player.MaxCp, CurCp = player.CurCp, MaxMp = player.MaxMp, CurMp = player.CurMp, Face = player.Face, HairStyle = player.HairStyle, HairColor = player.HairColor,
                Sex = player.Sex, Heading = player.Heading, X = player.X, Y = player.Y, Z = player.Z, Exp = player.Exp, ExpBeforeDeath = player.ExpBeforeDeath, Sp = player.Sp,
                Karma = player.Karma, PvpKills = player.PvpKills, PkKills = player.PkKills, ClanId = player.ClanId, Race = player.Race, classid = player.ClassId, base_class = player.BaseClass,
                DeleteTime = player.DeleteTime,
                CanCraft = player.CanCraft, Title = player.Title, rec_have = player.RecHave, rec_left = player.RecLeft, AccessLevel = player.AccessLevel, clan_privs = player.ClanPrivs,
                WantsPeace = player.WantsPeace, IsIn7sDungeon = player.IsIn7sDungeon, punish_level = player.PunishLevel, punish_timer = player.PunishTimer, power_grade = player.PowerGrade,
                Nobless = player.Nobless, Hero = player.Hero, Subpledge = player.Subpledge, last_recom_date = player.LastRecomDate, lvl_joined_academy = player.LevelJoinedAcademy,
                Apprentice = player.Apprentice, Sponsor = player.Sponsor, varka_ketra_ally = player.VarkaKetraAlly, clan_join_expiry_time = player.ClanJoinExpiryTime ,
                clan_create_expiry_time = player.ClanCreateExpiryTime, death_penalty_level = player.DeathPenaltyLevel
                });
        }
    }
}
