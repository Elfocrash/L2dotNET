using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMoveSpeed : StatFunction
    {
        public static FuncMoveSpeed Instance = new FuncMoveSpeed();

        private FuncMoveSpeed() : base(CharacterStatId.RunSpeed, 0x30)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.DexBonus[statFuncEnv.Character.CharacterStat.Dex]);
        }
    }
}