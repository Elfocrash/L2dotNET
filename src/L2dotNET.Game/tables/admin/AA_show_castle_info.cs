namespace L2dotNET.Game.tables.admin
{
    class AA_show_castle_info : _adminAlias
    {
        public AA_show_castle_info()
        {
            cmd = "show_castle_info";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //show_castle_info [castle_id] -- показывает статус замка
        }
    }
}
