namespace L2dotNET.GameService.tables.admin
{
    class AA_set_nobless : _adminAlias
    {
        public AA_set_nobless()
        {
            cmd = "set_nobless";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_nobless -- делает выбранного чара дворянином
        }
    }
}