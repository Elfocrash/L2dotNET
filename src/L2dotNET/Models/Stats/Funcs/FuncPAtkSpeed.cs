using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncPAtkSpeed : Func
    {
        public FuncPAtkSpeed() : base(Stats.PowerAttackSpeed, 0x20, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.MulValue(Formulas.DexBonus[env.Character.CharacterStat.Dex]);
        }
    }
}