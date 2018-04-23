﻿using L2dotNET.Models.Player;
using static L2dotNET.Models.Inventory.Inventory;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncPDefMod : Func
    {
        public FuncPDefMod() : base(Stats.PowerDefence, 0x20, null)
        {
        }

        public override void Calculate(Env env)
        {
            if (env.Character is L2Player player)
            {
                if(player.Inventory.GetPaperdollItem(PaperdollHead) != null)
                    env.SubValue(12);
                if (player.Inventory.GetPaperdollItem(PaperdollChest) != null)
                    env.SubValue(31);
                if (player.Inventory.GetPaperdollItem(PaperdollLegs) != null)
                    env.SubValue(18);
                if (player.Inventory.GetPaperdollItem(PaperdollGloves) != null)
                    env.SubValue(8);
                if (player.Inventory.GetPaperdollItem(PaperdollFeet) != null)
                    env.SubValue(7);
            }

            env.MulValue(env.Character.GetLevelMod());
        }
    }
}