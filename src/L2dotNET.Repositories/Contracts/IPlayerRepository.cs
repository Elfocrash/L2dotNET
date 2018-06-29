using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IPlayerRepository
    {
        CharacterContract GetPlayerByLogin(int objId);

        bool CheckIfPlayerNameExists(string name);

        void CreatePlayer(CharacterContract character);

        void UpdatePlayer(CharacterContract character);

        CharacterContract GetPlayerModelBySlotId(string accountName, int slotId);

        bool MarkToDeleteChar(int objId, long deletetime);

        bool MarkToRestoreChar(int objId);

        bool DeleteCharByObjId(int objId);
    }
}