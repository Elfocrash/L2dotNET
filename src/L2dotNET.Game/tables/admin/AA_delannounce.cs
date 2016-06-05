namespace L2dotNET.GameService.tables.admin
{
    class AA_delannounce : _adminAlias
    {
        public AA_delannounce()
        {
            cmd = "delannounce";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //delannounce [id] -- удаляет анонс , выдаваемый клиенту при подключении
        }
    }
}