namespace L2dotNET.GameService.tables.admin
{
    class AA_gmspeed : _adminAlias
    {
        public AA_gmspeed()
        {
            cmd = "gmspeed";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //gmspeed [1..5] -- повышает скорость бега
        }
    }
}