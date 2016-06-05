namespace L2dotNET.GameService.tables.admin
{
    class AA_set_castle_owner : _adminAlias
    {
        public AA_set_castle_owner()
        {
            cmd = "set_castle_owner";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_castle_owner [castle_id] [clan_id] -- устанавливает клан [clan_id] владельцем замка [castle_id]
        }
    }
}