using L2dotNET.Models;
using System.Collections.Generic;

namespace L2dotNET.Services.Contracts
{
    public interface IPlayerService
    {
        PlayerModel GetAccountByLogin(int objId);

        bool CheckIfPlayerNameExists(string name);

        void CreatePlayer(PlayerModel player);

        void UpdatePlayer(PlayerModel player);

        PlayerModel GetPlayerModelBySlotId(string accountName, int slotId);

        bool MarkToDeleteChar(int objId, long deletetime);

        bool MarkToRestoreChar(int objId);

        bool DeleteCharByObjId(int objId);

        List<SkillResponseModel> GetPlayerSkills(int objId);
    }
}