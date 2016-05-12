namespace L2dotNET.GameService.tables.admin
{
    class AA_set_hero : _adminAlias
    {
        public AA_set_hero()
        {
            cmd = "set_hero";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_hero -- делает выбранного чара героем
        }
    }
}
