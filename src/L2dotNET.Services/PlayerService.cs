using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using System.Collections.Generic;

namespace L2dotNET.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public bool MarkToDeleteChar(int objId, long deletetime)
        {
            return _unitOfWork.PlayerRepository.MarkToDeleteChar(objId, deletetime);
        }

        public bool MarkToRestoreChar(int objId)
        {
            return _unitOfWork.PlayerRepository.MarkToRestoreChar(objId);
        }

        public bool DeleteCharByObjId(int objId)
        {
            return _unitOfWork.PlayerRepository.DeleteCharByObjId(objId);
        }
        public List<SkillResponseModel> GetPlayerSkills(int objId)
        {
            return _unitOfWork.SkillRepository.GetPlayerSkills(objId);
        }
    }
}