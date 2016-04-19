namespace L2dotNET.Game.tables.admin
{
    class AA_setai : _adminAlias
    {
        public AA_setai()
        {
            cmd = "setai";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //setai [npcname]- дать выбранному мобу интеллект моба npcname, при этом учитывается аггрессивность
        }
    }
}
