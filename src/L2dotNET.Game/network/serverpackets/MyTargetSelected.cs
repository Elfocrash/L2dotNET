namespace L2dotNET.GameService.network.l2send
{
    class MyTargetSelected : GameServerNetworkPacket
    {
        private int _targetId;
        private short _color;

        public MyTargetSelected(int target, int color)
        {
            _targetId = target;
            _color = (short)color;
        }

        protected internal override void write()
        {
            writeC(0xa6);
            writeD(_targetId);
            writeH(_color);
        }
    }
}
