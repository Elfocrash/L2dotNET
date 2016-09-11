using L2dotNET.Attributes;
using L2dotNET.model.player;

namespace L2dotNET.Commands.Admin
{
    [AdminCommand(CommandName = "spawn")]
    class AdminSpawnNpc : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            //NpcTable.Instance.SpawnNpc(Convert.ToInt32(alias.Split(' ')[1]), admin.X, admin.Y, admin.Z, admin.Heading);
        }
    }
}