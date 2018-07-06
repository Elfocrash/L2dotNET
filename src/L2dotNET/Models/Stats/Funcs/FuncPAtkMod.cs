using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncPAtkMod : StatFunction
    {
        public static FuncPAtkMod Instance = new FuncPAtkMod();

        private FuncPAtkMod() : base(CharacterStatId.PowerAttack, 0x30)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.StrBonus[statFuncEnv.Character.CharacterStat.Str] * statFuncEnv.Character.GetLevelMod());
        }
    }
}