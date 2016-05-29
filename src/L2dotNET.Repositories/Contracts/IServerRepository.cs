using L2dotNET.Models;
using System.Collections.Generic;

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
