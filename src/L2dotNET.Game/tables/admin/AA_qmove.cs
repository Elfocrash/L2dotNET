namespace L2dotNET.Game.tables.admin
{
    class AA_qmove : _adminAlias
    {
        public AA_qmove()
        {
            cmd = "qmove";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //qmove -- не знаю что это у меня вылетает сервер )
        }
    }
}
