using L2dotNET.Game.managers;

namespace L2dotNET.Game.tables.admin
{
    class AA_transform : _adminAlias
    {
        public AA_transform()
        {
            cmd = "transform";
        }

        protected internal override void use(L2Player admin, string alias)
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
