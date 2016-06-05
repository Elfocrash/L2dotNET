using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Commands.Admin
{
    class AdminTeleport : AAdminCommand
    {
        public AdminTeleport()
        {
            Cmd = "teleport";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            int x = int.Parse(alias.Split(' ')[1]);
            int y = int.Parse(alias.Split(' ')[2]);
            int z = int.Parse(alias.Split(' ')[3]);

            admin.teleport(x, y, z);
        }
    }
}