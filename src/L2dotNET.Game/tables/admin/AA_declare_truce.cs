namespace L2dotNET.GameService.tables.admin
{
    class AA_declare_truce : _adminAlias
    {
        public AA_declare_truce()
        {
            cmd = "declare_truce";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //declare_truce [clan_1] [clan2] -- перемирие кланов [clan_1] , [clan_2]
        }
    }
}
