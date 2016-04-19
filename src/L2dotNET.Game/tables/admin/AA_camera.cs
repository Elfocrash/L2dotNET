namespace L2dotNET.Game.tables.admin
{
    class AA_camera : _adminAlias
    {
        public AA_camera()
        {
            cmd = "camera";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //camera [on|off] - Режим полета камеры. Лучше нажать alt+h после активации)
        }
    }
}
