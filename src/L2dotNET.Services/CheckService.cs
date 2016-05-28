using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Services
{
    public class CheckService : ICheckService
    {
        IUnitOfWork unitOfWork;

        public CheckService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool PreCheckRepository()
        {
            return this.unitOfWork.CheckRepository.PreCheckRepository();
        }
    }
}
