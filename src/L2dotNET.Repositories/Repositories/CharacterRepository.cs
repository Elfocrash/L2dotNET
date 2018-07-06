using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
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
                    return (await database.CountAsync<CharacterContract>($"WHERE Name = '{name}'")) == 1;
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(CheckIfPlayerNameExists)}. Message: '{ex.Message}'");
                throw;
            }
        }

        public async Task<CharacterContract> GetCharacterBySlot(int accountId, int slotId)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.GetSingleOrDefaultAsync<CharacterContract>($"WHERE AccountId = {accountId} AND CharSlot = {slotId}");
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(GetCharacterBySlot)}. Message: '{ex.Message}'");
                throw;
            }
        }

        public async Task<IEnumerable<CharacterContract>> GetCharactersOnAccount(int accountId)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.GetRangeAsync<CharacterContract>($"WHERE AccountId = {accountId}");
                }
            }
            catch (Exception ex)
            {
                Log.Halt($"Method: {nameof(GetCharacterBySlot)}. Message: '{ex.Message}'");
                throw;
            }
        }
    }
}