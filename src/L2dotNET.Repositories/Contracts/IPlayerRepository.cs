using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IPlayerRepository
    {
        PlayerContract GetPlayerByLogin(int objId);

        bool CheckIfPlayerNameExists(string name);

        void CreatePlayer(PlayerContract player);

        void UpdatePlayer(PlayerContract player);

        PlayerContract GetPlayerModelBySlotId(string accountName, int slotId);

        bool MarkToDeleteChar(int objId, long deletetime);

        bool MarkToRestoreChar(int objId);

        bool DeleteCharByObjId(int objId);
    }
}