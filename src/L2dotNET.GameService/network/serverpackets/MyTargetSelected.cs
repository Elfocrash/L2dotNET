namespace L2dotNET.GameService.Network.Serverpackets
{
    class MyTargetSelected : GameServerNetworkPacket
    {
        private readonly int _targetId;
        private readonly short _color;

        public MyTargetSelected(int target, int color)
        {
            _targetId = target;
            _color = (short)color;
        }

        protected internal override void Write()
        {
            WriteC(0xa6);
            WriteD(_targetId);
            WriteH(_color);
        }
    }
}