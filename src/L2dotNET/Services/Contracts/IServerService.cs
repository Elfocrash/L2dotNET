using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface IServerService
    {
        Task<IEnumerable<ServerContract>> GetServerList();

        Task<IEnumerable<AnnouncementContract>> GetAnnouncementsList();

        Task<IEnumerable<SpawnlistContract>> GetAllSpawns();
    }
}