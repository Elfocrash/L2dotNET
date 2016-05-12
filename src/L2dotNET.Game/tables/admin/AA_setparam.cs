namespace L2dotNET.GameService.tables.admin
{
    class AA_setparam : _adminAlias
    {
        public AA_setparam()
        {
            cmd = "setparam";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //setparam [param] [value] -- установить параметр (level - уровень, exp - опыт, sp - очки навыков, str, dex, int, con, wit, men - хар-ки)
            //setparam pk_counter -- установить счетчик пк
        }
    }
}
