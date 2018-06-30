using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Repositories.Utils;
using NLog;
using PeregrineDb;

namespace L2dotNET.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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