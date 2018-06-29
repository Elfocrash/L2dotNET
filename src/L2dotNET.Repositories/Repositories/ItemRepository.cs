using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Repositories.Contracts;
using MySql.Data.MySqlClient;

namespace L2dotNET.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        internal IDbConnection Db;

        public ItemRepository()
        {
            Db = new MySqlConnection("Server=127.0.0.1;Database=l2dotnet;Uid=l2dotnet;Pwd=l2dotnet;SslMode=none;");
        }

        public List<ItemContract> RestoreInventory(int objId, string location)
        {
            const string sql = @"SELECT object_id as ObjectId, item_id as ItemId, Count, enchant_level as Enchant, loc as Location, loc_data as LocationData,
                    custom_type1 as CustomType1, custom_type2 as CustomType2, mana_left as ManaLeft, Time, 1 as ExistsInDb FROM items WHERE owner_id=@owner_id AND (loc=@loc)";

            return Db.Query<ItemContract>(sql, new { owner_id = objId, loc = location }).ToList();
        }
    }
}