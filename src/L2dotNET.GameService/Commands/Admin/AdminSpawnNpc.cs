using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Commands.Admin
{
    class AdminSpawnNpc : AAdminCommand
    {
        public AdminSpawnNpc()
        {
            Cmd = "spawn";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            //NpcTable.Instance.SpawnNpc(Convert.ToInt32(alias.Split(' ')[1]), admin.X, admin.Y, admin.Z, admin.Heading);
        }
    }
}