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
            const string sql = @"SELECT item_id as ItemId, Name, BodyPart, Crystallizable, armor_type as ArmorType, Weight,
                    crystal_type as CrystalType, avoid_modify as AvoidModify, Duration, p_def as Pdef, m_def as Mdef, mp_bonus as MpBonus, Price, 
                    crystal_count as CrystalCount, Sellable, Dropable, Destroyable, Tradeable, item_skill_id as ItemSkillId, 
                    item_skill_lvl as ItemSkillLvl FROM armor";
            return Db.Query<ArmorModel>(sql).ToList();
        }

        public List<WeaponModel> GetAllWeapons()
        {
            const string sql = @"SELECT item_id as ItemId, Name, BodyPart, Crystallizable, Weight, Soulshots, Spiritshots,
                    crystal_type as CrystalType, p_dam as Pdam, rnd_dam as RndDam, WeaponType, Critical, hit_modify as HitModify, avoid_modify as AvoidModify,
                    shield_def as ShieldDef, shield_def_rate as ShieldDefRate, atk_speed as AtkSpeed, mp_consume as MpConsume, m_dam as Mdam, Duration, Price,
                    crystal_count as CrystalCount, Sellable, Dropable, Destroyable, Tradeable, item_skill_id as ItemSkillId, item_skill_lvl as ItemSkillLvl,
                    enchant4_skill_id as Enchant4SkillId,enchant4_skill_lvl as Enchant4SkillLvl, onCast_skill_id as OnCastSkillId, onCast_skill_lvl as OnCastSkillLvl,
                    onCast_skill_chance as OnCastSkillChance, onCrit_skill_id as OnCritSkillId, onCrit_skill_lvl as OnCritSkillLvl, onCrit_skill_chance as OnCritSkillChance FROM weapon";
            return Db.Query<WeaponModel>(sql).ToList();
        }

        public List<EtcItemModel> GetAllEtcItems()
        {
            const string sql =
                @"SELECT item_id as ItemId, Name, Crystallizable, item_type as ItemType, Weight, consume_type as ConsumeType, crystal_type as CrystalType, Duration, Price, 
                crystal_count as CrystalCount, Sellable, Dropable, Destroyable, Tradeable FROM etcitem";

            return Db.Query<EtcItemModel>(sql).ToList();
        }

        public void InsertNewItem(ItemModel item)
        {
            const string sql =
                @"Insert into items (owner_id,item_id,count,loc,loc_data,enchant_level,object_id,custom_type1,custom_type2,mana_left,time)
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

        public void UpdateItem(ItemModel item)
        {
            if (!item.ExistsInDb)
                return;

            const string sql = @"UPDATE items SET owner_id=@owner_id,count=@count,loc=@loc,loc_data=@loc_data,enchant_level=@enchant_level,custom_type1=@custom_type1,
                    custom_type2=@custom_type2,mana_left=@mana_left,time=@time WHERE object_id = @object_id";

            Db.ExecuteAsync(sql, new
            {
                owner_id = item.OwnerId,
                count = item.Count,
                loc = item.Location,
                loc_data = item.LocationData,
                enchant_level = item.Enchant,
                custom_type1 = item.CustomType1,
                custom_type2 = item.CustomType2,
                mana_left = item.ManaLeft,
                time = item.Time,
                object_id = item.ObjectId
            });
        }

        public List<ItemModel> RestoreInventory(int objId, string location)
        {
            const string sql = @"SELECT object_id as ObjectId, item_id as ItemId, Count, enchant_level as Enchant, loc as Location, loc_data as LocationData,
                    custom_type1 as CustomType1, custom_type2 as CustomType2, mana_left as ManaLeft, Time, 1 as ExistsInDb FROM items WHERE owner_id=@owner_id AND (loc=@loc)";

            return Db.Query<ItemModel>(sql, new { owner_id = objId, loc = location }).ToList();
        }
    }
}