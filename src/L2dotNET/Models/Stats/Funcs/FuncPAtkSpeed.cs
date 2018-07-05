﻿using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncPAtkSpeed : StatFunction
    {
        public static FuncPAtkSpeed Instance = new FuncPAtkSpeed();

        private FuncPAtkSpeed() : base(CharacterStatId.PowerAttackSpeed, 0x20)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            statFuncEnv.MulValue(Formulas.DexBonus[statFuncEnv.Character.CharacterStat.Dex]);
        }
    }
}