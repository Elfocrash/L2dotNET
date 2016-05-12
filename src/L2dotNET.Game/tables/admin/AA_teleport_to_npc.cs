namespace L2dotNET.GameService.tables.admin
{
    class AA_teleport_to_npc : _adminAlias
    {
        public AA_teleport_to_npc()
        {
            cmd = "teleport_to_npc";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //teleport_to_npc [NPC class id | NPC name] -- телепорт к нпц [NPC class id | NPC name] -- - не работает
        }
    }
}
