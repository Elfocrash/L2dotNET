namespace L2dotNET.GameService.tables.admin
{
    class AA_chat : _adminAlias
    {
        public AA_chat()
        {
            cmd = "chat";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            admin.ShowHtmAdmin("main.htm", false);
        }
    }
}