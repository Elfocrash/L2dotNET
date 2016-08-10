using L2dotNET.model.player;

namespace L2dotNET.Commands.Admin
{
    class AdminTest : AAdminCommand
    {
        public AdminTest()
        {
            Cmd = "test";
        }

        //private L2Player p = null;
        //private int spd = 1000;

        protected internal override void Use(L2Player admin, string alias)
        {
            //p = admin;
            //spd = int.Parse(alias.Split(' ')[1]);

            //if (!lp)
            //    lp = true;
            //else
            //    lp = false;

            //new System.Threading.Thread(loopme).Start();
        }

        //private bool lp = false;

        private void Loopme() { }
    }
}