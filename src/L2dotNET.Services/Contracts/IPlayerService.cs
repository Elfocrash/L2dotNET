using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Services.Contracts
{
    public interface IPlayerService
    {
        PlayerModel GetAccountByLogin(int objId);

        bool CheckIfPlayerNameExists(string name);

        void CreatePlayer(PlayerModel player);

        void UpdatePlayer(PlayerModel player);

        PlayerModel GetPlayerModelBySlotId(string accountName, int slotId);
    }
}
