namespace L2dotNET.GameService.tables.admin
{
    class AA_attack : _adminAlias
    {
        public AA_attack()
        {
            cmd = "attack";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //attack [castle_id] [clan_id] -- записывает клан [clan_id] на осаду замка [castle_id] атакующим
        }
    }
}
