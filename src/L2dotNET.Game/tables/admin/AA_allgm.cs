namespace L2dotNET.Game.tables.admin
{
    class AA_allgm : _adminAlias
    {
        public AA_allgm()
        {
            cmd = "allgm";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //allgm -- You get the online GM list with the BuilderLVL. Useful to trace illegal builders.
        }
    }
}
