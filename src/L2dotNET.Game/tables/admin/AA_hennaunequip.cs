namespace L2dotNET.GameService.tables.admin
{
    class AA_hennaunequip : _adminAlias
    {
        public AA_hennaunequip()
        {
            cmd = "hennaunequip";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //hennaunequip -- снять тату
        }
    }
}
