namespace L2dotNET.Game.tables.admin
{
    class AA_polymorph : _adminAlias
    {
        public AA_polymorph()
        {
            cmd = "polymorph";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //polymorph [npc id|npc name] -- превращает в моба [npc id|npc name]
        }
    }
}
