using L2dotNET.Models.Inventory;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.Basic;
using static L2dotNET.Models.Inventory.Inventory;

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
                if (player.Inventory.GetPaperdollItem(PaperdollLfinger) != null)
                    env.SubValue(5);
                if (player.Inventory.GetPaperdollItem(PaperdollRfinger) != null)
                    env.SubValue(5);
                if (player.Inventory.GetPaperdollItem(PaperdollLear) != null)
                    env.SubValue(9);
                if (player.Inventory.GetPaperdollItem(PaperdollRear) != null)
                    env.SubValue(9);
                if (player.Inventory.GetPaperdollItem(PaperdollNeck) != null)
                    env.SubValue(13);
            }

            env.MulValue(Formulas.MenBonus[env.Character.CharacterStat.Men] * env.Character.GetLevelMod());
        }
    }
}