namespace L2dotNET.GameService.tables.admin
{
    class AA_escape : _adminAlias
    {
        public AA_escape()
        {
            cmd = "escape";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //escape -- бесплатное сое в ближайший город
        }
    }
}
