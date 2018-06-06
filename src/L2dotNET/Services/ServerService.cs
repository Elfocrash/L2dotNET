using System.Collections.Generic;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class ServerService : IServerService
    {
        private readonly IServerRepository _serverRepository;

        public ServerService(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public List<ServerContract> GetServerList()
        {
            return _serverRepository.GetServerList();
        }

        public List<int> GetPlayersObjectIdList()
        {
            return _serverRepository.GetPlayersObjectIdList();
        }

        public List<int> GetPlayersItemsObjectIdList()
        {
            return _serverRepository.GetPlayersItemsObjectIdList();
        }

        public List<AnnouncementContract> GetAnnouncementsList()
        {
            return _serverRepository.GetAnnouncementsList();
        }

        public bool CheckDatabaseQuery()
        {
            return _serverRepository.CheckDatabaseQuery();
        }

        public List<SpawnlistContract> GetAllSpawns()
        {
            return _serverRepository.GetAllSpawns();
        }
    }
}