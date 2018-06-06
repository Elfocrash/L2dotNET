using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class CheckService : ICheckService
    {
        private readonly ICheckRepository _checkRepository;

        public CheckService(ICheckRepository checkRepository)
        {
            _checkRepository = checkRepository;
        }

        public bool PreCheckRepository()
        {
            return _checkRepository.PreCheckRepository();
        }
    }
}