using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMAtkMod : StatFunction
    {
        public static FuncMAtkMod Instance = new FuncMAtkMod();

        private FuncMAtkMod() : base(CharacterStatId.MagicAttack, 0x20)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            double intb = Formulas.IntBonus[statFuncEnv.Character.CharacterStat.Int];
            double lvlb = statFuncEnv.Character.GetLevelMod();
            statFuncEnv.MulValue((lvlb * lvlb) * (intb * intb));
        }
    }
}