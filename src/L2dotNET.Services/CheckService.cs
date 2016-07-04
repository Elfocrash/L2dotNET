using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class CheckService : ICheckService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool PreCheckRepository()
        {
            return _unitOfWork.CheckRepository.PreCheckRepository();
        }
    }
}