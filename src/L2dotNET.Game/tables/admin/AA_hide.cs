namespace L2dotNET.GameService.tables.admin
{
    class AA_hide : _adminAlias
    {
        public AA_hide()
        {
            cmd = "hide";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //hide [on|off] -- включение\отключение невидимости
        }
    }
}
