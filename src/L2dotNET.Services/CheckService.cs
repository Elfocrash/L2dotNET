using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class CheckService : ICheckService
    {
        private readonly IUnitOfWork unitOfWork;

        public CheckService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool PreCheckRepository()
        {
            return unitOfWork.CheckRepository.PreCheckRepository();
        }
    }
}