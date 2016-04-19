namespace L2dotNET.Game.tables.admin
{
    class AA_who : _adminAlias
    {
        public AA_who()
        {
            cmd = "who";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //who - показать online (сколько человек онлайн, макс онлайн, сколько ботов, сколько чаров продает что-либо)
        }
    }
}
