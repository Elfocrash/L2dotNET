namespace L2dotNET.GameService.tables.admin
{
    class AA_set_siege_period : _adminAlias
    {
        public AA_set_siege_period()
        {
            cmd = "set_siege_period";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_siege_period [castle_id] [время] -- период осады замка [castle_id] , [время] в секундах
        }
    }
}
