using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IServerRepository
    {
        List<int> GetPlayersObjectIdList();

        List<int> GetPlayersItemsObjectIdList();

        List<SpawnlistContract> GetAllSpawns();

        bool CheckDatabaseQuery();
    }
}