﻿using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Repositories.Utils;
using NLog;
using PeregrineDb;

namespace L2dotNET.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task<bool> CheckIfPlayerNameExists(string name)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return (await database.CountAsync<CharacterContract>($"WHERE Name = {name}")) == 1;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method: {nameof(CheckIfPlayerNameExists)}. Message: '{ex.Message}'");
                throw;
            }
        }

        public async Task<CharacterContract> GetPlayerModelBySlotId(string accountName, int slotId)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.GetSingleOrDefaultAsync<CharacterContract>($"WHERE AccountName = {accountName} AND CharSlot = {slotId}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method: {nameof(GetPlayerModelBySlotId)}. Message: '{ex.Message}'");
                throw;
            }
        }
    }
}