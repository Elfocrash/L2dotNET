using System;

namespace L2dotNET.GameService.tables.admin
{
    class AA_spawn : _adminAlias
    {
        public AA_spawn()
        {
            cmd = "spawn";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            NpcTable.Instance.SpawnNpc(Convert.ToInt32(alias.Split(' ')[1]), admin.X, admin.Y, admin.Z, admin.Heading);
        }
    }
}
