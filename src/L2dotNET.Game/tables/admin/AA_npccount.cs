namespace L2dotNET.GameService.tables.admin
{
    class AA_npccount : _adminAlias
    {
        public AA_npccount()
        {
            cmd = "npccount";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //npccount -- NPC's online spawned. Really useful when creating new areas with NPC's.
        }
    }
}
