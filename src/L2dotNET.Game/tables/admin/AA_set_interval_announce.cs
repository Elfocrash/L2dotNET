namespace L2dotNET.GameService.tables.admin
{
    class AA_set_interval_announce : _adminAlias
    {
        public AA_set_interval_announce()
        {
            cmd = "set_interval_announce";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_interval_announce add [int] [id] [текст] -- задает периодический анонс с периодом [int] (10,20,30 ...) (анонсы должны быть приостановлены в момент добавления\удаления)
            //set_interval_announce del [int] [id] [текст] -- удаляет периодический анонс с периодом [int] (10,20,30 ...) (анонсы должны быть приостановлены в момент добавления\удаления)
            //set_interval_announce start -- приостанавливает периодические анонсы
            //set_interval_announce end -- запускает периодические анонсы
        }
    }
}