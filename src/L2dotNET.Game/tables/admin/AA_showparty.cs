namespace L2dotNET.Game.tables.admin
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
