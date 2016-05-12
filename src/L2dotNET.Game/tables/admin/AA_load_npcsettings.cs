namespace L2dotNET.GameService.tables.admin
{
    class AA_load_npcsettings : _adminAlias
    {
        public AA_load_npcsettings()
        {
            cmd = "load_npcsettings";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //load_npcsettings -- расставляет эвент-менеджеров по городам
        }
    }
}
