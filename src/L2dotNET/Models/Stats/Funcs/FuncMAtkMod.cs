using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMAtkMod : Func
    {
        public FuncMAtkMod() : base(Stats.MagicAttack, 0x20, null)
        {
        }

        public override void Calculate(Env env)
        {
            double intb = Formulas.IntBonus[env.Character.CharacterStat.Int];
            double lvlb = env.Character.GetLevelMod();
            env.MulValue((lvlb * lvlb) * (intb * intb));
        }
    }
}