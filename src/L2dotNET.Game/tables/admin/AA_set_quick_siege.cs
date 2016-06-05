namespace L2dotNET.GameService.tables.admin
{
    class AA_set_quick_siege : _adminAlias
    {
        public AA_set_quick_siege()
        {
            cmd = "set_quick_siege";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_quick_siege [castle_id] [время] -- быстрая осада замка [castle_id] , [время] в секундах до начала осады
        }
    }
}