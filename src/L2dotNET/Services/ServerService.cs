using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Abstract;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class ServerService : IServerService
    {
        private readonly IServerRepository _serverRepository;
        private readonly ICrudRepository<AnnouncementContract> _announcementCrudRepository;
        private readonly ICrudRepository<ServerContract> _serverCrudRepository;

        public ServerService(IServerRepository serverRepository,
            ICrudRepository<AnnouncementContract> announcementCrudRepository,
            ICrudRepository<ServerContract> serverCrudRepository)
        {
            _serverRepository = serverRepository;
            _announcementCrudRepository = announcementCrudRepository;
            _serverCrudRepository = serverCrudRepository;
        }

        public async Task<IEnumerable<ServerContract>> GetServerList()
        {
            return await _serverCrudRepository.GetAll();
        }

        public List<int> GetPlayersObjectIdList()
        {
            return _serverRepository.GetPlayersObjectIdList();
        }

        public List<int> GetPlayersItemsObjectIdList()
        {
            return _serverRepository.GetPlayersItemsObjectIdList();
        }

        public async Task<IEnumerable<AnnouncementContract>> GetAnnouncementsList()
        {
            return await _announcementCrudRepository.GetAll();
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