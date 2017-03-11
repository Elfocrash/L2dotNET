using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Services.Contracts
{
    public interface IServerService
    {
        List<ServerModel> GetServerList();

        List<int> GetPlayersObjectIdList();

        List<AnnouncementModel> GetAnnouncementsList();

        List<SpawnlistModel> GetAllSpawns();

        bool CheckDatabaseQuery();
    }
}