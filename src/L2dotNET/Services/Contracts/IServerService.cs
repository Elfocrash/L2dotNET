using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface IServerService
    {
        Task<IEnumerable<ServerContract>> GetServerList();

        List<int> GetPlayersObjectIdList();

        List<int> GetPlayersItemsObjectIdList();

        Task<IEnumerable<AnnouncementContract>> GetAnnouncementsList();

        List<SpawnlistContract> GetAllSpawns();

        bool CheckDatabaseQuery();
    }
}