using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMAtkSpeed : Func
    {
        public FuncMAtkSpeed() : base(Stats.MagicAttackSpeed, 0x20, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.MulValue(Formulas.WitBonus[env.Character.Stats.Wit]);
        }
    }
}