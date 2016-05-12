namespace L2dotNET.GameService.tables.admin
{
    class AA_send_gmroom : _adminAlias
    {
        public AA_send_gmroom()
        {
            cmd = "send_gmroom";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //send_gmroom - оказатся в комнате для GM
        }
    }
}
