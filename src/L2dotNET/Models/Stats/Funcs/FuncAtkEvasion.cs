using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncAtkEvasion : StatFunction
    {
        public static FuncAtkEvasion Instance = new FuncAtkEvasion();

        private FuncAtkEvasion() : base(CharacterStatId.EvasionRate, 0x10)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(Formulas.BaseEvasionAccuracy[statFuncEnv.Character.CharacterStat.Dex] + statFuncEnv.Character.Level);
        }
    }
}