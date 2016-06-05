using L2dotNET.GameService.Commands;
using L2dotNET.GameService.managers;

namespace L2dotNET.GameService.Command
{
    class AdminTransform : AAdminCommand
    {
        public AdminTransform()
        {
            Cmd = "transform";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            if (alias.Split(' ')[1].Equals("on"))
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