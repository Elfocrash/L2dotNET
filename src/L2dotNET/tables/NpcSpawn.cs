namespace L2dotNET.tables
{
    class NpcSpawn
    {
        private static readonly NpcSpawn Ns = new NpcSpawn();

        public static NpcSpawn GetInstance()
        {
            return Ns;
        }
    }
}