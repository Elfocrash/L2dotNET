using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMAtkSpeed : StatFunction
    {
        public static FuncMAtkSpeed Instance = new FuncMAtkSpeed();

        private FuncMAtkSpeed() : base(CharacterStatId.MagicAttackSpeed, 0x20)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.WitBonus[statFuncEnv.Character.CharacterStat.Wit]);
        }
    }
}