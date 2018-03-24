using L2dotNET.Models.player;
using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMAtkCritical : Func
    {
        public FuncMAtkCritical() : base(Stats.McriticalRate, 0x30, null)
        {
        }

        public override void Calculate(Env env)
        {
            if (env.Character is L2Player player)
            {
                if(player.ActiveWeapon != null)
                    env.MulValue(Formulas.WitBonus[player.Stats.Wit]);
            }
            else
                env.MulValue(Formulas.WitBonus[env.Character.Stats.Wit]);
        }
    }
}