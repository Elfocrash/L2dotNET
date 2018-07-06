using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;

namespace L2dotNET.Models.Inventory
{
    public class PcInventory : Inventory
    {
        public PcInventory(L2Character owner)
            : base(owner)
        {
        }

        public bool ReduceAdena(int count, L2Player player)
        {
            return false;
        }

    }
}