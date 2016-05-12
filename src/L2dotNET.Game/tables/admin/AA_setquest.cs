namespace L2dotNET.GameService.tables.admin
{
    class AA_setquest : _adminAlias
    {
        public AA_setquest()
        {
            cmd = "setquest";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //setquest [quest id] [state] -- назначить цели квест по id, с состоянием state
        }
    }
}
