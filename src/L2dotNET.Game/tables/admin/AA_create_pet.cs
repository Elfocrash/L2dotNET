namespace L2dotNET.GameService.tables.admin
{
    class AA_create_pet : _adminAlias
    {
        public AA_create_pet()
        {
            cmd = "create_pet";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //create_pet [id] [lvl] -- создает питомца [id] уровня [lvl]
        }
    }
}
