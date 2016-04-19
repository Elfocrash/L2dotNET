namespace L2dotNET.Game.tables.admin
{
    class AA_set_siege : _adminAlias
    {
        public AA_set_siege()
        {
            cmd = "set_siege";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_siege [castle_id] [год] [месяц] [день] [часы] [минуты] -- задаёт начало осады замка
        }
    }
}
