namespace L2dotNET.GameService.tables.admin
{
    class AA_tradeon : _adminAlias
    {
        public AA_tradeon()
        {
            cmd = "tradeon";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //tradeon - включить трейд
        }
    }
}