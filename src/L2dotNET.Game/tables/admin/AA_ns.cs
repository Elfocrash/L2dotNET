namespace L2dotNET.Game.tables.admin
{
    class AA_ns : _adminAlias
    {
        public AA_ns()
        {
            cmd = "ns";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //ns hero 0 - убрать статус hero
            //ns noblesse 0 - убрать статус noblesse
        }
    }
}
