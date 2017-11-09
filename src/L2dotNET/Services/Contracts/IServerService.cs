using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface IServerService
    {
        List<ServerContract> GetServerList();

        List<int> GetPlayersObjectIdList();

        List<AnnouncementContract> GetAnnouncementsList();

        List<SpawnlistContract> GetAllSpawns();

        bool CheckDatabaseQuery();
    }
}