using System.Collections.Generic;
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
    public class ItemRepository : IItemRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemRepository));

        internal IDbConnection Db;

        public ItemRepository()
        {
            Db = new MySqlConnection(ConfigurationManager.ConnectionStrings["PrimaryConnection"].ToString());
        }

        public List<ArmorModel> GetAllArmors()
        {
            var sql = @"SELECT item_id as ItemId, Name, BodyPart, Crystallizable, armor_type as ArmorType, Weight,
                    crystal_type as CrystalType, avoid_modify as AvoidModify, Duration, p_def as Pdef, m_def as Mdef, mp_bonus as MpBonus, Price, 
                    crystal_count as CrystalCount, Sellable, Dropable, Destroyable, Tradeable, item_skill_id as ItemSkillId, 
                    item_skill_lvl as ItemSkillLvl FROM armor";
            return Db.Query<ArmorModel>(sql).ToList();
        }

        public void InsertNewItem(ItemModel item)
        {
            var sql = @"Insert into items (owner_id,item_id,count,loc,loc_data,enchant_level,object_id,custom_type1,custom_type2,mana_left,time)
                    Values (@owner_id,@item_id,@count,@loc,@loc_data,@enchant_level,@object_id,@custom_type1,@custom_type2,@mana_left,@time)";

            Db.ExecuteAsync(sql,
                new
                {
                    owner_id = item.OwnerId,
                    item_id = item.ItemId,
                    count = item.Count,
                    loc = item.Location,
                    loc_data = item.LocationData,
                    enchant_level = item.Enchant,
                    object_id = item.ObjectId,
                    custom_type1 = item.CustomType1,
                    custom_type2 = item.CustomType2,
                    mana_left = item.ManaLeft,
                    time = item.Time
                });
        }
    }
}