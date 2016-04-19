namespace L2dotNET.Game.tables.admin
{
    class AA_getbookmark : _adminAlias
    {
        public AA_getbookmark()
        {
            cmd = "getbookmark";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //getbookmark - получить список закладок (в клиенте видно не весь)
        }
    }
}
