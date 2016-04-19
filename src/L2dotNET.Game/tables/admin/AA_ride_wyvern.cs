namespace L2dotNET.Game.tables.admin
{
    class AA_ride_wyvern : _adminAlias
    {
        public AA_ride_wyvern()
        {
            cmd = "ride_wyvern";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //ride_wyvern -- оседлать виверна
        }
    }
}
