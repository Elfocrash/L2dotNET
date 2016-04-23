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
    public class PlayerService : IPlayerService
    {
        IUnitOfWork unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PlayerModel GetAccountByLogin(int objId)
        {
            return this.unitOfWork.PlayerRepository.GetAccountByLogin(objId);
        }

        public bool CheckIfPlayerNameExists(string name)
        {
            return this.unitOfWork.PlayerRepository.CheckIfPlayerNameExists(name);
        }

        public void CreatePlayer(PlayerModel player)
        {
            this.unitOfWork.PlayerRepository.CreatePlayer(player);
        }
    }
}
