using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMoveSpeed : Func
    {
        public FuncMoveSpeed() : base(Stats.RunSpeed, 0x30, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.MulValue(Formulas.DexBonus[env.Character.CharacterStat.Dex]);
        }
    }
}