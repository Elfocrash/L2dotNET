using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Repositories.Utils;
using PeregrineDb;

namespace L2dotNET.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        public async Task<AccountContract> GetAccountByLogin(string login)
        {
            try
            {
                using (IDatabase database = ConnectionFactory.Open())
                {
                    return await database.GetSingleOrDefaultAsync<AccountContract>($"WHERE Login = {login}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method: {nameof(GetAccountByLogin)}. Message: '{ex.Message}'");
                throw;
            }
        }
    }
}