namespace L2dotNET.GameService.tables.admin
{
    class AA_ssq_info : _adminAlias
    {
        public AA_ssq_info()
        {
            cmd = "ssq_info";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //ssq_info -- статус + немного инфо
        }
    }
}
