namespace L2dotNET.GameService.tables.admin
{
    class AA_actorcount : _adminAlias
    {
        public AA_actorcount()
        {
            cmd = "actorcount";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //actorcount -- You get the players' number and NPC's in the area.
        }
    }
}
