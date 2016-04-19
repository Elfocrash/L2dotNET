namespace L2dotNET.Game.tables.admin
{
    class AA_serverstat : _adminAlias
    {
        public AA_serverstat()
        {
            cmd = "serverstat";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //serverstat - показывает статистику сервера: время работы, онлайн и т.п..
        }
    }
}
