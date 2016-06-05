namespace L2dotNET.GameService.tables.admin
{
    class AA_announce : _adminAlias
    {
        public AA_announce()
        {
            cmd = "announce";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //announce -- аннонс слышится по всему миру... ограничен одной строчкой текста
        }
    }
}