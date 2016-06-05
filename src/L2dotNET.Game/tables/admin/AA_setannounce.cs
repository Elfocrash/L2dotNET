namespace L2dotNET.GameService.tables.admin
{
    class AA_setannounce : _adminAlias
    {
        public AA_setannounce()
        {
            cmd = "setannounce";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //setannounce [id] [текст] -- задает анонс, выдаваемый клиенту при подключении
        }
    }
}