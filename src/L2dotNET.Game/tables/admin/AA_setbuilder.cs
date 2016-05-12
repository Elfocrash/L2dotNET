namespace L2dotNET.GameService.tables.admin
{
    class AA_setbuilder : _adminAlias
    {
        public AA_setbuilder()
        {
            cmd = "setbuilder";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //setbuilder [charname] [level] -- назначить персонажа ГМом уровня level
            //1 level - полные права
            //2 level - команды работающие только на себя
            //3 level - только команды телепорта и общие ГМовские команды
        }
    }
}
