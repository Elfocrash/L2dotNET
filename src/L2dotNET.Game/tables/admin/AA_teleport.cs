namespace L2dotNET.GameService.tables.admin
{
    class AA_teleport : _adminAlias
    {
        public AA_teleport()
        {
            cmd = "teleport";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //teleport x y z -- телепорт в точку с координатами x y z
            int x = int.Parse(alias.Split(' ')[1]);
            int y = int.Parse(alias.Split(' ')[2]);
            int z = int.Parse(alias.Split(' ')[3]);

            admin.teleport(x, y, z);
        }
    }
}
