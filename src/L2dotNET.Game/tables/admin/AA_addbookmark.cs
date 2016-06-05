namespace L2dotNET.GameService.tables.admin
{
    class AA_addbookmark : _adminAlias
    {
        public AA_addbookmark()
        {
            cmd = "addbookmark";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //addbookmark - добавить закладку текущей локации
        }
    }
}