using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Services
{
    public class PlayerService : IPlayerService
    {
        IUnitOfWork unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //Dummy method
        public int GetDeviceIdByPlayerName(string name)
        {
            return this.unitOfWork.PlayerRepository.GetDeviceIdByPlayerName(name);
        }
    }
}
