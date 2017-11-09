using System.Collections.Generic;
using L2dotNET.DataContracts;
using L2dotNET.model.player;

namespace L2dotNET.Services.Contracts
{
    public interface IPlayerService
    {
        L2Player GetPlayerByLogin(int objId);

        bool CheckIfPlayerNameExists(string name);

        void CreatePlayer(L2Player player);

        void UpdatePlayer(L2Player player);

        L2Player GetPlayerBySlotId(string accountName, int slotId);

        bool MarkToDeleteChar(int objId, long deletetime);

        bool MarkToRestoreChar(int objId);

        bool DeleteCharByObjId(int objId);

        List<SkillResponseContract> GetPlayerSkills(int objId);

        L2Player RestorePlayer(int id, GameClient client);
    }
}