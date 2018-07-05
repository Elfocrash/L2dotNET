using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using static L2dotNET.Models.Inventory.Inventory;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncPDefMod : StatFunction
    {
        public static FuncPDefMod Instance = new FuncPDefMod();

        private FuncPDefMod() : base(CharacterStatId.PowerDefence, 0x20)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            if (statFuncEnv.Character is L2Player player)
            {
                if(player.Inventory.GetPaperdollItem(PaperdollHead) != null)
                    statFuncEnv.SubValue(12);
                if (player.Inventory.GetPaperdollItem(PaperdollChest) != null)
                    statFuncEnv.SubValue(31);
                if (player.Inventory.GetPaperdollItem(PaperdollLegs) != null)
                    statFuncEnv.SubValue(18);
                if (player.Inventory.GetPaperdollItem(PaperdollGloves) != null)
                    statFuncEnv.SubValue(8);
                if (player.Inventory.GetPaperdollItem(PaperdollFeet) != null)
                    statFuncEnv.SubValue(7);
            }

            statFuncEnv.MulValue(statFuncEnv.Character.GetLevelMod());
        }
    }
}