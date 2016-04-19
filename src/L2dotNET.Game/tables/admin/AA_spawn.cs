using System;

namespace L2dotNET.Game.tables.admin
{
    class AA_spawn : _adminAlias
    {
        public AA_spawn()
        {
            cmd = "spawn";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            NpcTable.getInstance().spawnNpc(Convert.ToInt32(alias.Split(' ')[1]), admin.X, admin.Y, admin.Z, admin.Heading);
        }
    }
}
