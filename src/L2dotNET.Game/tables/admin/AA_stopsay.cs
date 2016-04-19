namespace L2dotNET.Game.tables.admin
{
    class AA_stopsay : _adminAlias
    {
        public AA_stopsay()
        {
            cmd = "stopsay";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //stopsay [ник] [время] -- запрет чата чару [ник] на [время] минут
        }
    }
}
