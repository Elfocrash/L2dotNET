namespace L2dotNET.Game.tables.admin
{
    class AA_kick : _adminAlias
    {
        public AA_kick()
        {
            cmd = "kick";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //kick [charname] -- выкидывает из игры чара [charname]
        }
    }
}
