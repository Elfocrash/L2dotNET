using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMaxCpMul : Func
    {
        public FuncMaxCpMul() : base(Stats.MaxCp, 0x20, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.MulValue(Formulas.ConBonus[env.Character.Con]);
        }
    }
}