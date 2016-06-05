namespace L2dotNET.GameService.network.l2send
{
    class CharMoveToLocationMonrace : GameServerNetworkPacket
    {
        //private MonsterRunner runner;
        //public CharMoveToLocationMonrace(MonsterRunner runner)
        //{
        //    this.runner = runner;
        //}

        protected internal override void write()
        {
            writeC(0x2f);

            //writeD(runner.id);

            //writeD(runner.dx);
            //writeD(runner.dy);
            //writeD(runner.dz);

            //writeD(runner.x);
            //writeD(runner.y);
            //writeD(runner.z);
        }
    }
}
