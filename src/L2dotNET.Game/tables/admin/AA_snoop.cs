namespace L2dotNET.Game.tables.admin
{
    class AA_snoop : _adminAlias
    {
        public AA_snoop()
        {
            cmd = "snoop";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //snoop [charname | .] [ON | OFF] -- включить слежение за сообщениями персонажа
        }
    }
}
