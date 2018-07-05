using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMaxHpMul : StatFunction
    {
        public static FuncMaxHpMul Instance = new FuncMaxHpMul();

        private FuncMaxHpMul() : base(CharacterStatId.MaxHp, 0x20)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.ConBonus[statFuncEnv.Character.CharacterStat.Con]);
        }
    }
}