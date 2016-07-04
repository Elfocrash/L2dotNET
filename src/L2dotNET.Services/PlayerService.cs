using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public PlayerModel GetAccountByLogin(int objId)
        {
            return _unitOfWork.PlayerRepository.GetAccountByLogin(objId);
        }

        public bool CheckIfPlayerNameExists(string name)
        {
            return _unitOfWork.PlayerRepository.CheckIfPlayerNameExists(name);
        }

        public void CreatePlayer(PlayerModel player)
        {
            _unitOfWork.PlayerRepository.CreatePlayer(player);
        }

        public void UpdatePlayer(PlayerModel player)
        {
            _unitOfWork.PlayerRepository.UpdatePlayer(player);
        }

        public PlayerModel GetPlayerModelBySlotId(string accountName, int slotId)
        {
            return _unitOfWork.PlayerRepository.GetPlayerModelBySlotId(accountName, slotId);
        }

        public bool MarkToDeleteChar(int objId)
        {
            return _unitOfWork.PlayerRepository.MarkToDeleteChar(objId);
        }

        public bool DeleteCharByObjId(int objId)
        {
            return _unitOfWork.PlayerRepository.DeleteCharByObjId(objId);
        }
    }
}