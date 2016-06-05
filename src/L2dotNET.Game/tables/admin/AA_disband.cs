namespace L2dotNET.GameService.tables.admin
{
    class AA_disband : _adminAlias
    {
        public AA_disband()
        {
            cmd = "disband";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //disband [clan_name] -- распускает клан [clan_name]
        }
    }
}