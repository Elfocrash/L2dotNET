namespace L2dotNET.GameService.tables.admin
{
    class AA_disband2 : _adminAlias
    {
        public AA_disband2()
        {
            cmd = "disband2";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //disband2 [alliance_name] -- распускает альянс [alliance_name]
        }
    }
}
