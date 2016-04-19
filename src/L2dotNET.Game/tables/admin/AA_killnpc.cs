namespace L2dotNET.Game.tables.admin
{
    class AA_killnpc : _adminAlias
    {
        public AA_killnpc()
        {
            cmd = "killnpc";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //killnpc -- убивает выбранного NPC
        }
    }
}
