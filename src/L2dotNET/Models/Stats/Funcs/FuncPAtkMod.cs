using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncPAtkMod : Func
    {
        public FuncPAtkMod() : base(Stats.PowerAttack, 0x30, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.MulValue(Formulas.StrBonus[env.Character.Stats.Str] * env.Character.GetLevelMod());
        }
    }
}