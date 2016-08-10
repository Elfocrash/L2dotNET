namespace L2dotNET.Network.serverpackets
{
    class CharMoveToLocationMonrace : GameserverPacket
    {
        //private MonsterRunner runner;
        //public CharMoveToLocationMonrace(MonsterRunner runner)
        //{
        //    this.runner = runner;
        //}

        public override void Write()
        {
            WriteByte(0x2f);

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