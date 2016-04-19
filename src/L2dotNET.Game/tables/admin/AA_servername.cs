namespace L2dotNET.Game.tables.admin
{
    class AA_servername : _adminAlias
    {
        public AA_servername()
        {
            cmd = "servername";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //servername - показывает имя сервера
        }
    }
}
