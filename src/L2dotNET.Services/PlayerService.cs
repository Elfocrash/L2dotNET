using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PlayerModel GetAccountByLogin(int objId)
        {
            return unitOfWork.PlayerRepository.GetAccountByLogin(objId);
        }

        public bool CheckIfPlayerNameExists(string name)
        {
            return unitOfWork.PlayerRepository.CheckIfPlayerNameExists(name);
        }

        public void CreatePlayer(PlayerModel player)
        {
            unitOfWork.PlayerRepository.CreatePlayer(player);
        }

        public void UpdatePlayer(PlayerModel player)
        {
            unitOfWork.PlayerRepository.UpdatePlayer(player);
        }

        public PlayerModel GetPlayerModelBySlotId(string accountName, int slotId)
        {
            return unitOfWork.PlayerRepository.GetPlayerModelBySlotId(accountName, slotId);
        }

        public bool MarkToDeleteChar(int objId)
        {
            return unitOfWork.PlayerRepository.MarkToDeleteChar(objId);
        }

        public bool DeleteCharByObjId(int objId)
        {
            return unitOfWork.PlayerRepository.DeleteCharByObjId(objId);
        }
    }
}