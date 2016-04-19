namespace L2dotNET.Game.tables.admin
{
    class AA_home : _adminAlias
    {
        public AA_home()
        {
            cmd = "home";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //home -- телепорт на ТИ
        }
    }
}
