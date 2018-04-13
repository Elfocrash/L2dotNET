using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncAtkCritical : Func
    {
        public FuncAtkCritical() : base(Stats.CriticalRate, 0x09, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.MulValue(Formulas.DexBonus[env.Character.CharacterStat.Dex]);
            env.MulValue(10);
            env.BaseValue = env.Value;
        }
    }
}