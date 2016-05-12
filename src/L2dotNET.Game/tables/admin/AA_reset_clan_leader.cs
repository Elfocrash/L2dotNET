namespace L2dotNET.GameService.tables.admin
{
    class AA_reset_clan_leader : _adminAlias
    {
        public AA_reset_clan_leader()
        {
            cmd = "reset_clan_leader";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //reset_clan_leader [clan] [leader_name] -- назначает чара [leader_name] лидером клана [clan]
        }
    }
}
