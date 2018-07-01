using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IAccountRepository
    {
        Task<AccountContract> GetAccountByLogin(string login);
    }
}