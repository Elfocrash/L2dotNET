using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMaxMpMul : Func
    {
        public FuncMaxMpMul() : base(Stats.MaxMp, 0x20, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.MulValue(Formulas.MenBonus[env.Character.Stats.Men]);
        }
    }
}