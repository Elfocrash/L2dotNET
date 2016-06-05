namespace L2dotNET.GameService.tables
{
    class NpcSpawn
    {
        private static readonly NpcSpawn ns = new NpcSpawn();
        public static NpcSpawn getInstance()
        {
            return ns;
        }
    }
}
