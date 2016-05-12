namespace L2dotNET.GameService.tables.admin
{
    class AA_showparty : _adminAlias
    {
        public AA_showparty()
        {
            cmd = "showparty";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //showparty -- показать кто в партии
        }
    }
}
