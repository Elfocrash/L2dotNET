namespace L2dotNET.GameService.tables.admin
{
    class AA_hennaequip : _adminAlias
    {
        public AA_hennaequip()
        {
            cmd = "hennaequip";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //hennaequip -- одеть тату
        }
    }
}
