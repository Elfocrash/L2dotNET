using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Repositories.Contracts
{
    public interface IServerRepository
    {
        List<ServerModel> GetServerList();

        List<int> GetPlayersObjectIdList();

        List<AnnouncementModel> GetAnnouncementsList();

        bool CheckDatabaseQuery();
    }
}