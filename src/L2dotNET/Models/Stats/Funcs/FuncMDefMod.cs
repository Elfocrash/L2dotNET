using L2dotNET.Models.inventory;
using L2dotNET.Models.player;
using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMDefMod : Func
    {
        public FuncMDefMod() : base(Stats.MagicDefence, 0x20, null)
        {
        }


        public override void Calculate(Env env)
        {
            if (env.Character is L2Player player)
            {
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollLfinger) != null)
                    env.SubValue(5);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollRfinger) != null)
                    env.SubValue(5);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollLear) != null)
                    env.SubValue(9);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollRear) != null)
                    env.SubValue(9);
                if (player.Inventory.GetPaperdollItem(Inventory.PaperdollNeck) != null)
                    env.SubValue(13);
            }

            env.MulValue(Formulas.MenBonus[env.Character.Stats.Men] * env.Character.GetLevelMod());
        }
    }
}