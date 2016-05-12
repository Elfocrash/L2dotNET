namespace L2dotNET.GameService.tables.admin
{
    class AA_defend : _adminAlias
    {
        public AA_defend()
        {
            cmd = "defend";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //defend [castle_id] [clan_id] -- записывает клан [clan_id] на осаду замка [castle_id] защищающим
        }
    }
}
