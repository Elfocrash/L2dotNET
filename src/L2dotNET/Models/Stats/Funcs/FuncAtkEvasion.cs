using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncAtkEvasion : Func
    {
        public FuncAtkEvasion() : base(Stats.EvasionRate, 0x10, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.AddValue(Formulas.BaseEvasionAccuracy[env.Character.Stats.Dex] + env.Character.Level);
        }
    }
}