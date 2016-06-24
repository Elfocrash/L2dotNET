using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Commands.Admin
{
    class AdminTransform : AAdminCommand
    {
        public AdminTransform()
        {
            Cmd = "transform";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            if (alias.Split(' ')[1].EqualsIgnoreCase("on"))
            {
                int id = int.Parse(alias.Split(' ')[2]);
                int seconds = int.Parse(alias.Split(' ')[3]);
                TransformManager.getInstance().transformTo(id, admin, seconds);
            }
            else
            {
                admin.untransform();
            }
        }
    }
}