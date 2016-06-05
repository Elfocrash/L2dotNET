namespace L2dotNET.GameService.tables.admin
{
    class AA_set_siege_end : _adminAlias
    {
        public AA_set_siege_end()
        {
            cmd = "set_siege_end";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_siege_end [castle_id] [год] [месяц] [день] [часы] [минуты] -- задаёт конец осады замка
        }
    }
}