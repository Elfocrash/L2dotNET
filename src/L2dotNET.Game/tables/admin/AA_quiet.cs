namespace L2dotNET.Game.tables.admin
{
    class AA_quiet : _adminAlias
    {
        public AA_quiet()
        {
            cmd = "quiet";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //quiet -- нельзя писать никаких сообщений кроме аннонсов во всем мире
        }
    }
}
