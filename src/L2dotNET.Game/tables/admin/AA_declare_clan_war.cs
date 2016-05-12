namespace L2dotNET.GameService.tables.admin
{
    class AA_declare_clan_war : _adminAlias
    {
        public AA_declare_clan_war()
        {
            cmd = "declare_clan_war";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //declare_clan_war [clan_1] [clan2] -- начинает войну клана [clan_1] с [clan_2]
        }
    }
}
