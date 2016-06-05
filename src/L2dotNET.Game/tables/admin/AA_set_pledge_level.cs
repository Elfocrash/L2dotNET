namespace L2dotNET.GameService.tables.admin
{
    class AA_set_pledge_level : _adminAlias
    {
        public AA_set_pledge_level()
        {
            cmd = "set_pledge_level";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_pledge_level [clan] [lvl] -- устанавливает уровень [lvl] клану [clan]
        }
    }
}