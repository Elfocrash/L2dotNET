namespace L2dotNET.GameService.tables.admin
{
    class AA_reset_clan_name : _adminAlias
    {
        public AA_reset_clan_name()
        {
            cmd = "reset_clan_name";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //reset_clan_name [clan_name] [new_clan_name] -- меняет название клана [clan_name] на [new_clan_name]
        }
    }
}
