using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.Basic;

namespace L2dotNET.Models.Stats.Funcs
{
    public class FuncMAtkCritical : StatFunction
    {
        public static FuncMAtkCritical Instance = new FuncMAtkCritical();

        private FuncMAtkCritical() : base(CharacterStatId.McriticalRate, 0x30)
        {
        }

        public override void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            if (statFuncEnv.Character is L2Player player)
            {
                if(player.ActiveWeapon != null)
                {
                    statFuncEnv.MulValue(Formulas.WitBonus[player.CharacterStat.Wit]);
                }
            }
            else
            {
                statFuncEnv.MulValue(Formulas.WitBonus[statFuncEnv.Character.CharacterStat.Wit]);
            }
        }
    }
}