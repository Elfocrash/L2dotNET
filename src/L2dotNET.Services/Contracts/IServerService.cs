using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
