namespace L2dotNET.GameService.Tables
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