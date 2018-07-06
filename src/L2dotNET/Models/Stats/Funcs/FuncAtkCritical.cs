using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncAtkCritical : StatFunction
    {
        public static FuncAtkCritical Instance = new FuncAtkCritical();

        private FuncAtkCritical() : base(CharacterStatId.CriticalRate, 0x09)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.DexBonus[statFuncEnv.Character.CharacterStat.Dex]);
            statFuncEnv.MulValue(10);
            statFuncEnv.BaseValue = statFuncEnv.Value;
        }
    }
}