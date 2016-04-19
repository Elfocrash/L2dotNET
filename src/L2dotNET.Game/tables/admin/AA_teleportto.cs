namespace L2dotNET.Game.tables.admin
{
    class AA_teleportto : _adminAlias
    {
        public AA_teleportto()
        {
            cmd = "teleportto";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //teleportto [charname] -- телепорт к чару [charname]
        }
    }
}
