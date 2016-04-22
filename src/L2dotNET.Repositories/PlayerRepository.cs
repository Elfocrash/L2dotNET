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
    }
}
