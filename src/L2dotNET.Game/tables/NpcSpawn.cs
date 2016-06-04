namespace L2dotNET.GameService.tables
{
    class NpcSpawn
    {
        private static NpcSpawn ns = new NpcSpawn();
        public static NpcSpawn getInstance()
        {
            return ns;
        }
    }
}
