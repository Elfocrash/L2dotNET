namespace L2dotNET.GameService.tables.admin
{
    class AA_check_bot : _adminAlias
    {
        public AA_check_bot()
        {
            cmd = "check_bot";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //check_bot [1..3] -- проверка на бота выбранного чара
        }
    }
}
