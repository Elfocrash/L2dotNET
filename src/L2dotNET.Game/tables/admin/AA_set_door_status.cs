namespace L2dotNET.Game.tables.admin
{
    class AA_set_door_status : _adminAlias
    {
        public AA_set_door_status()
        {
            cmd = "set_door_status";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_door_status <имя_двери> <статус> - открывает/закрывает двери
//например, закрывает двери колизея: 
//set_door_status aden_colosseum_001_002 close
//set_door_status aden_colosseum_002_001 close
        }
    }
}
