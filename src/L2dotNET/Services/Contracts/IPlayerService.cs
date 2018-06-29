using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Services.Contracts
{
    public interface IPlayerService
    {
        Task<L2Player> GetPlayerByLogin(int characterId);

        Task<bool> CheckIfPlayerNameExists(string name);

        void CreatePlayer(L2Player player);

        void UpdatePlayer(L2Player player);

        Task<L2Player> GetPlayerBySlotId(string accountName, int slotId);

        bool DeleteCharByObjId(int characterId);

        Task<L2Player> RestorePlayer(int id, GameClient client);

        int GetDaysRequiredToDeletePlayer();
    }
}