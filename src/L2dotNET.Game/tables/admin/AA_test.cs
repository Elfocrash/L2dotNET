
using L2dotNET.GameService.network.l2send;
using System.Threading;
using System;
using L2dotNET.GameService.model.zones;
using L2dotNET.GameService.model.zones.forms;
namespace L2dotNET.GameService.tables.admin
{
    class AA_test : _adminAlias
    {
        public AA_test()
        {
            cmd = "test";
        }

        L2Player p = null;
        int spd = 1000;
        protected internal override void use(L2Player admin, string alias)
        {

            //p = admin;
            //spd = int.Parse(alias.Split(' ')[1]);

            //if (!lp)
            //    lp = true;
            //else
            //    lp = false;

            //new System.Threading.Thread(loopme).Start();
        }

        bool lp = false;

        private void loopme()
        {

        }
    }
}
