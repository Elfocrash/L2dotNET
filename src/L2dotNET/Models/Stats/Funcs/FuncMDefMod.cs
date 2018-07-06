using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.Basic;
using static L2dotNET.Models.Inventory.Inventory;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMDefMod : StatFunction
    {
        public static FuncMDefMod Instance = new FuncMDefMod();

        private FuncMDefMod() : base(CharacterStatId.MagicDefence, 0x20)
        {
        }


        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            if (statFuncEnv.Character is L2Player player)
            {
                if (player.Inventory.GetPaperdollItem(PaperdollLfinger) != null)
                    statFuncEnv.SubValue(5);
                if (player.Inventory.GetPaperdollItem(PaperdollRfinger) != null)
                    statFuncEnv.SubValue(5);
                if (player.Inventory.GetPaperdollItem(PaperdollLear) != null)
                    statFuncEnv.SubValue(9);
                if (player.Inventory.GetPaperdollItem(PaperdollRear) != null)
                    statFuncEnv.SubValue(9);
                if (player.Inventory.GetPaperdollItem(PaperdollNeck) != null)
                    statFuncEnv.SubValue(13);
            }

            statFuncEnv.MulValue(Formulas.MenBonus[statFuncEnv.Character.CharacterStat.Men] * statFuncEnv.Character.GetLevelMod());
        }
    }
}