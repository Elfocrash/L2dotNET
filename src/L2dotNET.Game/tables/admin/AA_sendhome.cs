namespace L2dotNET.GameService.tables.admin
{
    class AA_sendhome : _adminAlias
    {
        public AA_sendhome()
        {
            cmd = "sendhome";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //sendhome [ник] -- отправляет чара с ником [ник] в ближайший город
        }
    }
}