using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncAtkAccuracy : StatFunction
    {
        public static FuncAtkAccuracy Instance = new FuncAtkAccuracy();

        private FuncAtkAccuracy() : base(CharacterStatId.AccuracyCombat, 0x10)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.AddValue(Formulas.BaseEvasionAccuracy[statFuncEnv.Character.CharacterStat.Dex] + statFuncEnv.Character.Level);
        }
    }
}