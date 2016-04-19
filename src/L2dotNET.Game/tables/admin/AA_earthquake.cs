namespace L2dotNET.Game.tables.admin
{
    class AA_earthquake : _adminAlias
    {
        public AA_earthquake()
        {
            cmd = "earthquake";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //earthquake -- имитирует землятрясение (типа землетясение антараса)
        }
    }
}
