using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Repositories.Utils;
using NLog;
using PeregrineDb;

namespace L2dotNET.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task<IEnumerable<ItemContract>> RestoreInventory(int characterId)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.GetRangeAsync<ItemContract>($"WHERE CharacterId = {characterId} AND Location = {(int)ItemLocation.Inventory}");
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(RestoreInventory)}. Message: '{ex.Message}'");
                throw;
            }

        }

        public int GetMaxItemId()
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    int maxItemId = database.ExecuteScalar<int>("SELECT max(ItemId) FROM Items;");
                    int maxCharacterId = database.ExecuteScalar<int>("SELECT max(CharacterId) FROM Characters;");
                    return Math.Max(maxCharacterId, maxItemId);
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(GetMaxItemId)}. Message: '{ex.Message}'");
                throw;
            }

        }
    }
}