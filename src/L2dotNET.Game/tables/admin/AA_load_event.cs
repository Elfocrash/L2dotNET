namespace L2dotNET.GameService.tables.admin
{
    class AA_load_event : _adminAlias
    {
        public AA_load_event()
        {
            cmd = "load_event";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //load_event -- загружает первый эвент из файла eventdata.ini
        }
    }
}