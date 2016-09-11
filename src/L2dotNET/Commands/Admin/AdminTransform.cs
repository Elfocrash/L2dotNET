using L2dotNET.Attributes;
using L2dotNET.managers;
using L2dotNET.model.player;
using L2dotNET.Utility;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "transform")]
    class AdminTransform : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            if (alias.Split(' ')[1].EqualsIgnoreCase("on"))
            {
                int id = int.Parse(alias.Split(' ')[2]);
                int seconds = int.Parse(alias.Split(' ')[3]);
                TransformManager.GetInstance().TransformTo(id, admin, seconds);
            }
            else
                admin.Untransform();
        }
    }
}