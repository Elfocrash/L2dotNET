using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMaxCpMul : StatFunction
    {
        public static FuncMaxCpMul Instance = new FuncMaxCpMul();

        private FuncMaxCpMul() : base(CharacterStatId.MaxCp, 0x20)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.ConBonus[statFuncEnv.Character.Con]);
        }
    }
}