using System.Collections.Generic;
using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class ServerService : IServerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ServerModel> GetServerList()
        {
            return _unitOfWork.ServerRepository.GetServerList();
        }

        public List<int> GetPlayersObjectIdList()
        {
            return _unitOfWork.ServerRepository.GetPlayersObjectIdList();
        }

        public List<AnnouncementModel> GetAnnouncementsList()
        {
            return _unitOfWork.ServerRepository.GetAnnouncementsList();
        }

        public bool CheckDatabaseQuery()
        {
            return _unitOfWork.ServerRepository.CheckDatabaseQuery();
        }

        public List<SpawnlistModel> GetAllSpawns()
        {
            return _unitOfWork.ServerRepository.GetAllSpawns();
        }
    }
}