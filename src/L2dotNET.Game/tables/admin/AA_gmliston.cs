namespace L2dotNET.GameService.tables.admin
{
    class AA_gmliston : _adminAlias
    {
        public AA_gmliston()
        {
            cmd = "gmliston";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //gmliston -- включить себя в список ГМов показываемых при команде /gmlist
        }
    }
}