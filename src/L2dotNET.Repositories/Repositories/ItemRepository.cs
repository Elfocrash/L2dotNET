using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
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
                    return await database.GetRangeAsync<ItemContract>($"WHERE CharacterId = {characterId} AND Location = {ItemLocation.Inventory}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method: {nameof(RestoreInventory)}. Message: '{ex.Message}'");
                throw;
            }

        }
    }
}