using L2dotNET.Models.Player;
using L2dotNET.Models.Player.Basic;

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
                    env.MulValue(Formulas.WitBonus[player.CharacterStat.Wit]);
            }
            else
                env.MulValue(Formulas.WitBonus[env.Character.CharacterStat.Wit]);
        }
    }
}