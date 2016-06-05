namespace L2dotNET.GameService.tables.admin
{
    class AA_test : _adminAlias
    {
        public AA_test()
        {
            cmd = "test";
        }

        private L2Player p = null;
        private int spd = 1000;

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

        private bool lp = false;

        private void loopme() { }
    }
}