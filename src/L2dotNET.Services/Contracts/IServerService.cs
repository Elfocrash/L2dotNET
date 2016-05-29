using L2dotNET.Models;
using System.Collections.Generic;

namespace L2dotNET.Services.Contracts
{
    public interface IServerService
    {
        List<ServerModel> GetServerList();

        List<int> GetPlayersObjectIdList();

        List<AnnouncementModel> GetAnnouncementsList();

        bool CheckDatabaseQuery();
    }
}
