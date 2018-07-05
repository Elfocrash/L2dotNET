using L2dotNET.DataContracts.Shared.Enums;

namespace L2dotNET.Models.Stats
{
    public abstract class StatFunction
    {
        public readonly CharacterStatId Stat;
        public readonly int Order;

        protected StatFunction(CharacterStatId stat, int order)
        {
            Stat = stat;
            Order = order;
        }

        public abstract void Calculate(StatFunctionEnvironment statFuncEnv);
    }
}