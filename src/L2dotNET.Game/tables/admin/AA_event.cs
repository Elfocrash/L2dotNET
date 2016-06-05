namespace L2dotNET.GameService.tables.admin
{
    class AA_event : _adminAlias
    {
        public AA_event()
        {
            cmd = "event";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //event [type] [on|off] -- включает \ отключает эвент типа [type] ..... бестолковая )
        }
    }
}