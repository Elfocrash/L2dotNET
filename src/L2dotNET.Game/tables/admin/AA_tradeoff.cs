namespace L2dotNET.Game.tables.admin
{
    class AA_tradeoff : _adminAlias
    {
        public AA_tradeoff()
        {
            cmd = "tradeoff";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //tradeoff - выключить трейд
        }
    }
}
