namespace L2dotNET.Models.Stats
{
    public abstract class Func
    {
        public readonly Stat Stat;
        public readonly int Order;
        public readonly object FuncOwner;

        protected Func(Stat stat, int order, object funcOwner)
        {
            Stat = stat;
            Order = order;
            FuncOwner = funcOwner;
        }

        public abstract void Calculate(Env env);
    }
}