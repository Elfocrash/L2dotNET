using L2dotNET.Models.player.basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncAtkAccuracy : Func
    {
        public FuncAtkAccuracy() : base(Stats.AccuracyCombat, 0x10, null)
        {
        }

        public override void Calculate(Env env)
        {
            env.AddValue(Formulas.BaseEvasionAccuracy[env.Character.CharacterStat.Dex] + env.Character.Level);
        }
    }
}