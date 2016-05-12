namespace L2dotNET.GameService.tables.admin
{
    class AA_char_stop : _adminAlias
    {
        public AA_char_stop()
        {
            cmd = "char_stop";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //char_stop 9999 - заморозить чара 9999 мин
        }
    }
}
