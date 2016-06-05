namespace L2dotNET.GameService.tables.admin
{
    class AA_undying : _adminAlias
    {
        public AA_undying()
        {
            cmd = "undying";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //undying [on|off] -- включение\отключение бессмертия
        }
    }
}