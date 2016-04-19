namespace L2dotNET.Game.tables.admin
{
    class AA_killme : _adminAlias
    {
        public AA_killme()
        {
            cmd = "killme";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //killme - суицид. Могут вывалиться вещи.
        }
    }
}
