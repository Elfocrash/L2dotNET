using System.Reflection.Emit;
using L2dotNET.model.inventory;
using L2dotNET.model.player;

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
                if(player.Inventory.GetPaperdollItem(Inventory.PaperdollHead) != null)
                    env.SubValue(12);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollChest) != null)
                    env.SubValue(31);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollLegs) != null)
                    env.SubValue(18);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollGloves) != null)
                    env.SubValue(8);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollFeet) != null)
                    env.SubValue(7);
            }

            env.MulValue(env.Character.GetLevelMod());
        }
    }
}