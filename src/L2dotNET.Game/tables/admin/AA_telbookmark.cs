namespace L2dotNET.GameService.tables.admin
{
    class AA_telbookmark : _adminAlias
    {
        public AA_telbookmark()
        {
            cmd = "telbookmark";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //telbookmark - портанутся на закладку
        }
    }
}
