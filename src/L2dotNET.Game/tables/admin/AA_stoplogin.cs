namespace L2dotNET.GameService.tables.admin
{
    class AA_stoplogin : _adminAlias
    {
        public AA_stoplogin()
        {
            cmd = "stoplogin";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //stoplogin [nick name] [sec] - бан чара
        }
    }
}