using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMaxMpMul : StatFunction
    {
        public static FuncMaxMpMul Instance = new FuncMaxMpMul();

        private FuncMaxMpMul() : base(CharacterStatId.MaxMp, 0x20)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.MenBonus[statFuncEnv.Character.CharacterStat.Men]);
        }
    }
}