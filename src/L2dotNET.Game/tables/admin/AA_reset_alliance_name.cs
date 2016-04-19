namespace L2dotNET.Game.tables.admin
{
    class AA_reset_alliance_name : _adminAlias
    {
        public AA_reset_alliance_name()
        {
            cmd = "reset_alliance_name";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //reset_alliance_name [alliance_name] [new_alliance_name] -- меняет название альянса [alliance_name] на [new_alliance_name]
        }
    }
}
