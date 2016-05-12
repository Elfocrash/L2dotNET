namespace L2dotNET.GameService.tables.admin
{
    class AA_reset_castle_owner : _adminAlias
    {
        public AA_reset_castle_owner()
        {
            cmd = "reset_castle_owner";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //reset_castle_owner [castle_id] -- освобождает замок [castle_id]
        }
    }
}
